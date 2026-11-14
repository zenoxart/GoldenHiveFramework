using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.ModuleContracts;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace GoldenHive.NET.WinUI.Framework.Docking;

/// <summary>
/// Hosts tool windows docked at one side: a header (title, float, hide) and tabs when several tool
/// windows share the pane.
/// </summary>
public sealed class ToolWindowPane : Grid
{
    private readonly DockManager _manager;
    private readonly TextBlock _title = new() { VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.SemiBold, TextTrimming = TextTrimming.CharacterEllipsis };

    internal ToolWindowPane(DockManager manager, DockSide side)
    {
        _manager = manager;
        Side = side;
        RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        if (Application.Current.Resources.TryGetValue("LayerFillColorDefaultBrush", out var layer) && layer is Brush brush)
        {
            Background = brush;
        }

        var header = new Grid { Padding = new Thickness(12, 4, 4, 4), ColumnSpacing = 2 };
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        header.Children.Add(_title);

        var floatButton = CreateHeaderButton("detach", "shell:dock.float");
        floatButton.Click += (_, _) =>
        {
            if (SelectedItem is { } item)
            {
                _manager.Float(item);
            }
        };
        SetColumn(floatButton, 1);
        header.Children.Add(floatButton);

        var hideButton = CreateHeaderButton("close", "shell:dock.hidepane");
        hideButton.Click += (_, _) => _manager.TogglePane(Side);
        SetColumn(hideButton, 2);
        header.Children.Add(hideButton);
        Children.Add(header);

        Tabs = new TabView
        {
            IsAddTabButtonVisible = false,
            CanDragTabs = true,
            CanReorderTabs = true,
            TabWidthMode = TabViewWidthMode.SizeToContent,
        };
        Tabs.TabCloseRequested += (_, e) =>
        {
            if (e.Tab.Tag is DockItem item)
            {
                _manager.Close(item);
            }
        };
        Tabs.TabDroppedOutside += (_, e) =>
        {
            if (e.Tab.Tag is DockItem item)
            {
                _manager.Float(item);
            }
        };
        Tabs.SelectionChanged += (_, _) =>
        {
            UpdateTitle();
            if (SelectedItem is { } item)
            {
                _manager.OnItemActivated(item);
            }
        };
        SetRow(Tabs, 1);
        Children.Add(Tabs);
    }

    public DockSide Side { get; }

    public TabView Tabs { get; }

    /// <summary>False when the user hid the pane.</summary>
    public bool IsShown { get; set; } = true;

    public bool HasItems => Tabs.TabItems.Count > 0;

    public DockItem? SelectedItem => (Tabs.SelectedItem as TabViewItem)?.Tag as DockItem;

    public void UpdateTitle() => _title.Text = SelectedItem?.Title ?? string.Empty;

    private static Button CreateHeaderButton(string icon, string toolTipKey)
    {
        var button = new Button
        {
            Padding = new Thickness(6),
            Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
            BorderThickness = new Thickness(0),
        };
        Ui.SetShowLabel(button, false);
        Ui.SetIcon(button, icon);
        Ui.SetText(button, toolTipKey);
        return button;
    }
}
