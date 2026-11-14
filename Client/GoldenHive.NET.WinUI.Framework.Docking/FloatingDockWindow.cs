using GoldenHive.NET.WinUI.Framework.Commands;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace GoldenHive.NET.WinUI.Framework.Docking;

/// <summary>
/// A floating window of the dock layout (a detached view). Its monitor and position are saved in
/// AppData by the WindowManager and restored on the next start.
/// </summary>
public sealed class FloatingDockWindow : Window
{
    private readonly TextBlock _title;

    internal FloatingDockWindow(DockManager manager, DockItem item)
    {
        Item = item;
        Title = item.Title;
        SystemBackdrop = new MicaBackdrop();

        var root = new Grid();
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        var bar = new Grid { Padding = new Thickness(12, 6, 8, 6), ColumnSpacing = 8 };
        bar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        bar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        _title = new TextBlock { Text = item.Title, VerticalAlignment = VerticalAlignment.Center, TextTrimming = TextTrimming.CharacterEllipsis, Opacity = 0.8 };
        bar.Children.Add(_title);

        var dockBack = new Button();
        Ui.SetIcon(dockBack, "attach");
        Ui.SetText(dockBack, "shell:dock.dockback");
        dockBack.Click += (_, _) => manager.Dock(item);
        Grid.SetColumn(dockBack, 1);
        bar.Children.Add(dockBack);
        root.Children.Add(bar);

        var content = new Border { Child = item.View };
        Grid.SetRow(content, 1);
        root.Children.Add(content);
        Content = root;

        AppWindow.SetIcon("Assets/GoldenHive.ico");
        Activated += (_, e) =>
        {
            if (e.WindowActivationState != WindowActivationState.Deactivated)
            {
                manager.OnItemActivated(item);
            }
        };
        Closed += (_, _) =>
        {
            content.Child = null;
            manager.OnFloatingClosed(this);
        };
    }

    public DockItem Item { get; }

    /// <summary>True while the window closes because its item is docked back or the app shuts down.</summary>
    internal bool IsClosingSilently { get; private set; }

    internal void CloseSilently()
    {
        IsClosingSilently = true;
        Close();
    }

    internal void UpdateTitle()
    {
        Title = Item.Title;
        _title.Text = Item.Title;
    }
}
