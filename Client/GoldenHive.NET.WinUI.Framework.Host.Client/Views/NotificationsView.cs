using System.Globalization;
using GoldenHive.NET.WinUI.Framework.NotificationClient;
using GoldenHive.NET.WinUI.Framework.NotificationContracts;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Views;

/// <summary>Tool window with the notification centre (built in code).</summary>
public sealed partial class NotificationsView : UserControl
{
    private readonly NotificationsViewModel _vm;
    private readonly StackPanel _items = new() { Spacing = 6 };

    public NotificationsView(NotificationsViewModel vm)
    {
        _vm = vm;
        var markRead = new Button { Content = vm.T("notifications.markread") };
        markRead.Click += (_, _) => vm.MarkAllRead();
        var clear = new Button { Content = vm.T("notifications.clear") };
        clear.Click += (_, _) => vm.Clear();
        var toolbar = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
        toolbar.Children.Add(markRead);
        toolbar.Children.Add(clear);
        var root = new StackPanel { Spacing = 10, Padding = new Thickness(12) };
        root.Children.Add(toolbar);
        root.Children.Add(_items);
        Content = new ScrollViewer { Content = root };

        Fill();
        vm.Changed += (_, _) => DispatcherQueue.TryEnqueue(Fill);
    }

    private void Fill()
    {
        _items.Children.Clear();
        if (_vm.Items.Count == 0)
        {
            _items.Children.Add(new TextBlock { Text = _vm.T("notifications.none"), Opacity = 0.6 });
            return;
        }

        foreach (var item in _vm.Items)
        {
            _items.Children.Add(Card(item));
        }
    }

    private FrameworkElement Card(InAppNotification item)
    {
        var glyph = item.Severity switch
        {
            NotificationSeverity.Success => "\uE73E",
            NotificationSeverity.Warning => "\uE7BA",
            NotificationSeverity.Error => "\uEA39",
            _ => "\uE946",
        };
        var text = new StackPanel { Spacing = 2 };
        text.Children.Add(new TextBlock { Text = item.Title, FontWeight = item.IsRead ? FontWeights.Normal : FontWeights.SemiBold, TextWrapping = TextWrapping.Wrap });
        if (!string.IsNullOrEmpty(item.Message))
        {
            text.Children.Add(new TextBlock { Text = item.Message, TextWrapping = TextWrapping.Wrap, Opacity = 0.8 });
        }

        text.Children.Add(new TextBlock { Text = item.Time.ToString("g", CultureInfo.CurrentCulture), Opacity = 0.6, FontSize = 11 });
        var row = new Grid { ColumnSpacing = 10 };
        row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        row.Children.Add(new FontIcon { Glyph = glyph, FontSize = 16, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(0, 2, 0, 0) });
        Grid.SetColumn(text, 1);
        row.Children.Add(text);

        var button = new Button
        {
            Content = row,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            Padding = new Thickness(10),
            IsEnabled = item.NavigationTarget is not null || !item.IsRead,
        };
        button.Click += (_, _) => _vm.Open(item);
        return button;
    }
}
