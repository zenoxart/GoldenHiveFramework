using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.ModuleContracts;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace GoldenHive.NET.WinUI.Framework.Docking;

/// <summary>
/// Renders the dock layout of the main window: tool panes left, right and bottom with resize handles
/// and the document well in the centre. Responsive: in Medium the side panes, in Compact all panes
/// become overlays - the saved layout is not changed by that.
/// </summary>
public sealed class DockHost : Grid
{
    private const double SplitterSize = 4;
    private readonly DockManager _manager;
    private readonly ColumnDefinition _leftColumn = new();
    private readonly ColumnDefinition _leftSplitterColumn = new();
    private readonly ColumnDefinition _centerColumn = new() { Width = new GridLength(1, GridUnitType.Star) };
    private readonly ColumnDefinition _rightSplitterColumn = new();
    private readonly ColumnDefinition _rightColumn = new();
    private readonly RowDefinition _bottomSplitterRow = new();
    private readonly RowDefinition _bottomRow = new();
    private readonly Grid _center = new();
    private readonly TextBlock _emptyText;
    private readonly DockSplitter _leftSplitter;
    private readonly DockSplitter _rightSplitter;
    private readonly DockSplitter _bottomSplitter;
    private readonly Grid _overlay = new() { Visibility = Visibility.Collapsed };
    private readonly Border _overlayPanel = new();
    private SizeClass _sizeClass = SizeClass.Expanded;
    private DockSide? _overlaySide;

    internal DockHost(DockManager manager)
    {
        _manager = manager;
        ColumnDefinitions.Add(_leftColumn);
        ColumnDefinitions.Add(_leftSplitterColumn);
        ColumnDefinitions.Add(_centerColumn);
        ColumnDefinitions.Add(_rightSplitterColumn);
        ColumnDefinitions.Add(_rightColumn);
        _center.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star), MinHeight = 120 });
        _center.RowDefinitions.Add(_bottomSplitterRow);
        _center.RowDefinitions.Add(_bottomRow);

        Documents = new TabView
        {
            IsAddTabButtonVisible = false,
            CanDragTabs = true,
            CanReorderTabs = true,
            TabWidthMode = TabViewWidthMode.Equal,
        };
        Documents.TabCloseRequested += (_, e) =>
        {
            // The TabView is still processing the close click: changing TabItems synchronously here throws
            // ArgumentException in WinUI, so the close runs after the event.
            if (e.Tab.Tag is DockItem item)
            {
                DispatcherQueue.TryEnqueue(() => _manager.Close(item));
            }
        };
        Documents.TabDroppedOutside += (_, e) =>
        {
            if (e.Tab.Tag is DockItem item)
            {
                _manager.Float(item);
            }
        };
        Documents.SelectionChanged += (_, _) =>
        {
            UpdateEmptyState();
            if (ActiveDocument is { } item)
            {
                _manager.OnItemActivated(item);
            }
        };

        _emptyText = new TextBlock
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            MaxWidth = 360,
            Opacity = 0.6,
        };
        Ui.SetText(_emptyText, "shell:dock.empty");

        var documentArea = new Grid();
        documentArea.Children.Add(Documents);
        documentArea.Children.Add(_emptyText);
        _center.Children.Add(documentArea);

        Left = new ToolWindowPane(manager, DockSide.Left);
        Right = new ToolWindowPane(manager, DockSide.Right);
        Bottom = new ToolWindowPane(manager, DockSide.Bottom);

        _leftSplitter = new DockSplitter(true, () => LeftWidth, v => { LeftWidth = v; _leftColumn.Width = new GridLength(v); }, +1, 160, 900);
        _rightSplitter = new DockSplitter(true, () => RightWidth, v => { RightWidth = v; _rightColumn.Width = new GridLength(v); }, -1, 160, 900);
        _bottomSplitter = new DockSplitter(false, () => BottomHeight, v => { BottomHeight = v; _bottomRow.Height = new GridLength(v); }, -1, 80, 900);
        foreach (var splitter in new[] { _leftSplitter, _rightSplitter, _bottomSplitter })
        {
            splitter.DragCompleted += (_, _) => _manager.OnLayoutChanged();
        }

        SetRow(_bottomSplitter, 1);
        _center.Children.Add(_bottomSplitter);

        SetColumn(_leftSplitter, 1);
        SetColumn(_center, 2);
        SetColumn(_rightSplitter, 3);
        Children.Add(_leftSplitter);
        Children.Add(_center);
        Children.Add(_rightSplitter);

        // Overlay for panes that cannot be docked in the current size class.
        var backdrop = new Grid();
        if (Application.Current.Resources.TryGetValue("SmokeFillColorDefaultBrush", out var smoke) && smoke is Brush smokeBrush)
        {
            backdrop.Background = smokeBrush;
        }

        backdrop.Tapped += (_, _) => CloseOverlay();
        _overlay.Children.Add(backdrop);
        _overlay.Children.Add(_overlayPanel);
        SetColumnSpan(_overlay, 5);
        Children.Add(_overlay);

        UpdateEmptyState();
        Layout();
    }

    public TabView Documents { get; }

    public ToolWindowPane Left { get; }

    public ToolWindowPane Right { get; }

    public ToolWindowPane Bottom { get; }

    public double LeftWidth { get; set; } = 280;

    public double RightWidth { get; set; } = 300;

    public double BottomHeight { get; set; } = 220;

    public DockItem? ActiveDocument => (Documents.SelectedItem as TabViewItem)?.Tag as DockItem;

    public ToolWindowPane? PaneFor(DockSide side) => side switch
    {
        DockSide.Left => Left,
        DockSide.Right => Right,
        DockSide.Bottom => Bottom,
        _ => null,
    };

    /// <summary>True if the pane is visible (docked or as overlay).</summary>
    public bool IsPaneVisible(DockSide side) => PaneFor(side) is { } pane && pane.HasItems && (IsDocked(pane) || _overlaySide == side);

    internal void ApplySizeClass(SizeClass sizeClass)
    {
        _sizeClass = sizeClass;
        _overlaySide = null;
        Layout();
    }

    internal void Add(DockItem item, DockSide side, bool select)
    {
        var tab = CreateTab(item);
        item.Tab = tab;
        var tabView = side == DockSide.DocumentWell ? Documents : PaneFor(side)!.Tabs;
        tabView.TabItems.Add(tab);
        if (select || tabView.TabItems.Count == 1)
        {
            tabView.SelectedItem = tab;
        }

        if (PaneFor(side) is { } pane)
        {
            pane.UpdateTitle();
        }

        UpdateEmptyState();
        Layout();
    }

    internal void Remove(DockItem item)
    {
        if (item.Tab is { } tab)
        {
            foreach (var tabView in new[] { Documents, Left.Tabs, Right.Tabs, Bottom.Tabs })
            {
                RemoveTab(tabView, tab);
            }

            tab.Content = null;
            item.Tab = null;
        }

        item.View = null;
        Left.UpdateTitle();
        Right.UpdateTitle();
        Bottom.UpdateTitle();
        UpdateEmptyState();
        Layout();
    }

    /// <summary>Removes a tab by index; tolerates the TabView rejecting the change while it is busy (drag, close animation).</summary>
    private static void RemoveTab(TabView tabView, TabViewItem tab)
    {
        var index = tabView.TabItems.IndexOf(tab);
        if (index < 0)
        {
            return;
        }

        try
        {
            tabView.TabItems.RemoveAt(index);
        }
        catch (ArgumentException)
        {
            tabView.DispatcherQueue.TryEnqueue(() =>
            {
                var retry = tabView.TabItems.IndexOf(tab);
                if (retry >= 0)
                {
                    tabView.TabItems.RemoveAt(retry);
                }
            });
        }
    }

    internal void Select(DockItem item)
    {
        if (item.Tab is null)
        {
            return;
        }

        if (item.Location == DockSide.DocumentWell)
        {
            Documents.SelectedItem = item.Tab;
            return;
        }

        if (PaneFor(item.Location) is { } pane)
        {
            pane.Tabs.SelectedItem = item.Tab;
            pane.IsShown = true;
            if (!CanDock(item.Location))
            {
                _overlaySide = item.Location;
            }

            Layout();
        }
    }

    internal void UpdateTitle(DockItem item)
    {
        if (item.Tab is { } tab)
        {
            tab.Header = item.Title;
            ToolTipService.SetToolTip(tab, item.Title);
        }

        Left.UpdateTitle();
        Right.UpdateTitle();
        Bottom.UpdateTitle();
    }

    internal void CycleDocument(int step)
    {
        var count = Documents.TabItems.Count;
        if (count < 2)
        {
            return;
        }

        var index = Math.Max(0, Documents.SelectedIndex);
        Documents.SelectedIndex = ((index + step) % count + count) % count;
    }

    /// <summary>Shows or hides a pane; in small windows it opens as overlay instead.</summary>
    internal void TogglePane(DockSide side)
    {
        var pane = PaneFor(side);
        if (pane is null || !pane.HasItems)
        {
            return;
        }

        if (CanDock(side))
        {
            pane.IsShown = !pane.IsShown;
        }
        else
        {
            _overlaySide = _overlaySide == side ? null : side;
        }

        Layout();
    }

    internal void ApplyState(DockLayoutState state)
    {
        LeftWidth = state.LeftWidth;
        RightWidth = state.RightWidth;
        BottomHeight = state.BottomHeight;
        Left.IsShown = state.LeftShown;
        Right.IsShown = state.RightShown;
        Bottom.IsShown = state.BottomShown;
        Layout();
    }

    internal void ResetSizes()
    {
        LeftWidth = 280;
        RightWidth = 300;
        BottomHeight = 220;
        Left.IsShown = Right.IsShown = Bottom.IsShown = true;
        _overlaySide = null;
        Layout();
    }

    private bool CanDock(DockSide side) => side == DockSide.Bottom ? _sizeClass >= SizeClass.Medium : _sizeClass >= SizeClass.Expanded;

    private bool IsDocked(ToolWindowPane pane) => pane.HasItems && pane.IsShown && CanDock(pane.Side);

    private void CloseOverlay()
    {
        _overlaySide = null;
        Layout();
    }

    private void Layout()
    {
        if (_overlaySide is { } overlaySide && !(PaneFor(overlaySide)?.HasItems ?? false))
        {
            _overlaySide = null;
        }

        PlaceSidePane(Left, _leftColumn, _leftSplitterColumn, _leftSplitter, 0, LeftWidth);
        PlaceSidePane(Right, _rightColumn, _rightSplitterColumn, _rightSplitter, 4, RightWidth);

        var bottomDocked = IsDocked(Bottom);
        Detach(Bottom);
        if (bottomDocked)
        {
            SetRow(Bottom, 2);
            _center.Children.Add(Bottom);
        }

        _bottomRow.Height = new GridLength(bottomDocked ? BottomHeight : 0);
        _bottomSplitterRow.Height = new GridLength(bottomDocked ? SplitterSize : 0);
        _bottomSplitter.Visibility = bottomDocked ? Visibility.Visible : Visibility.Collapsed;

        UpdateOverlay();
    }

    private void PlaceSidePane(ToolWindowPane pane, ColumnDefinition column, ColumnDefinition splitterColumn, DockSplitter splitter, int columnIndex, double width)
    {
        var docked = IsDocked(pane);
        Detach(pane);
        if (docked)
        {
            SetColumn(pane, columnIndex);
            SetRow(pane, 0);
            Children.Insert(0, pane);
        }

        column.Width = new GridLength(docked ? width : 0);
        splitterColumn.Width = new GridLength(docked ? SplitterSize : 0);
        splitter.Visibility = docked ? Visibility.Visible : Visibility.Collapsed;
    }

    private void UpdateOverlay()
    {
        if (_overlaySide is not { } side || PaneFor(side) is not { } pane || IsDocked(pane))
        {
            _overlay.Visibility = Visibility.Collapsed;
            _overlayPanel.Child = null;
            return;
        }

        Detach(pane);
        _overlayPanel.Child = pane;
        _overlayPanel.HorizontalAlignment = side switch
        {
            DockSide.Left => HorizontalAlignment.Left,
            DockSide.Right => HorizontalAlignment.Right,
            _ => HorizontalAlignment.Stretch,
        };
        _overlayPanel.VerticalAlignment = side == DockSide.Bottom ? VerticalAlignment.Bottom : VerticalAlignment.Stretch;
        var available = ActualWidth > 0 ? ActualWidth : 800;
        _overlayPanel.Width = side == DockSide.Bottom ? double.NaN : Math.Min(side == DockSide.Left ? LeftWidth : RightWidth, available * 0.85);
        _overlayPanel.Height = side == DockSide.Bottom ? Math.Min(BottomHeight, Math.Max(160, ActualHeight * 0.6)) : double.NaN;
        _overlay.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// Removes a pane from every container it can live in. FrameworkElement.Parent is not reliable for
    /// elements added to panels in code, so all known containers are checked explicitly.
    /// </summary>
    private void Detach(FrameworkElement element)
    {
        var index = Children.IndexOf(element);
        if (index >= 0)
        {
            Children.RemoveAt(index);
        }

        index = _center.Children.IndexOf(element);
        if (index >= 0)
        {
            _center.Children.RemoveAt(index);
        }

        if (ReferenceEquals(_overlayPanel.Child, element))
        {
            _overlayPanel.Child = null;
        }
    }

    private void UpdateEmptyState() =>
        _emptyText.Visibility = Documents.TabItems.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

    private TabViewItem CreateTab(DockItem item)
    {
        var tab = new TabViewItem
        {
            Header = item.Title,
            Tag = item,
            IsClosable = item.Descriptor.CanClose,
            Content = item.View,
        };
        ToolTipService.SetToolTip(tab, item.Title);
        if (item.Icon is not null && _manager.Icons is { } icons)
        {
            tab.IconSource = icons.CreateIconSource(item.Icon);
        }

        var menu = new MenuFlyout();
        if (item.Descriptor.CanFloat)
        {
            var floatItem = new MenuFlyoutItem();
            Ui.SetIcon(floatItem, "detach");
            Ui.SetText(floatItem, "shell:dock.float");
            floatItem.Click += (_, _) => _manager.Float(item);
            menu.Items.Add(floatItem);
        }

        if (item.Descriptor.CanClose)
        {
            var closeItem = new MenuFlyoutItem();
            Ui.SetIcon(closeItem, "close");
            Ui.SetText(closeItem, "shell:dock.close");
            closeItem.Click += (_, _) => _manager.Close(item);
            menu.Items.Add(closeItem);
        }

        if (item.Descriptor.Role == ViewRole.Document)
        {
            var closeOthers = new MenuFlyoutItem();
            Ui.SetText(closeOthers, "shell:dock.closeothers");
            closeOthers.Click += (_, _) => _manager.CloseOtherDocuments(item);
            menu.Items.Add(closeOthers);
        }

        if (menu.Items.Count > 0)
        {
            tab.ContextFlyout = menu;
        }

        return tab;
    }
}
