using System.ComponentModel;
using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace GoldenHive.NET.WinUI.Framework.ModuleLoader.Sdk;

/// <summary>Small helpers to build views in code (modules use viewMode codeOnly - no XAML metadata needed).</summary>
public static class UiKit
{
    public static T Resource<T>(string key)
        where T : class => (T)Application.Current.Resources[key];

    public static TextBlock Text(string text, string? style = null, bool wrap = false) =>
        new()
        {
            Text = text,
            Style = style is null ? null : Resource<Style>(style),
            TextWrapping = wrap ? TextWrapping.Wrap : TextWrapping.NoWrap,
            TextTrimming = wrap ? TextTrimming.None : TextTrimming.CharacterEllipsis,
            VerticalAlignment = VerticalAlignment.Center,
        };

    public static TextBlock Title(string text) => Text(text, "SubtitleTextBlockStyle");

    public static TextBlock Caption(string text) => Text(text, "CaptionTextBlockStyle", wrap: true);

    public static TextBlock Label(string text) => Text(text, "BodyStrongTextBlockStyle");

    /// <summary>A button bound to a command of the central command manager (icon, text, tooltip, shortcut, enabled state).</summary>
    public static Button CommandButton(string commandId, object? parameter = null, bool showLabel = true)
    {
        var button = new Button { CommandParameter = parameter };
        Ui.SetShowLabel(button, showLabel);
        Ui.SetCommand(button, commandId);
        return button;
    }

    public static Button Button(string text, RoutedEventHandler click, bool accent = false, string? glyph = null)
    {
        object content = text;
        if (glyph is not null)
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            panel.Children.Add(new FontIcon { Glyph = glyph, FontSize = 14 });
            panel.Children.Add(new TextBlock { Text = text });
            content = panel;
        }

        var button = new Button { Content = content };
        if (accent)
        {
            button.Style = Resource<Style>("AccentButtonStyle");
        }

        button.Click += click;
        return button;
    }

    public static StackPanel Row(params UIElement[] children) => Stack(Orientation.Horizontal, 8, children);

    public static StackPanel Column(params UIElement[] children) => Stack(Orientation.Vertical, 8, children);

    public static StackPanel Stack(Orientation orientation, double spacing, params UIElement[] children)
    {
        var panel = new StackPanel { Orientation = orientation, Spacing = spacing };
        foreach (var child in children)
        {
            panel.Children.Add(child);
        }

        return panel;
    }

    /// <summary>A rounded card on the layer background.</summary>
    public static Border Card(UIElement content, double padding = 16) => new()
    {
        Child = content,
        Padding = new Thickness(padding),
        CornerRadius = new CornerRadius(8),
        Background = Resource<Brush>("CardBackgroundFillColorDefaultBrush"),
        BorderBrush = Resource<Brush>("CardStrokeColorDefaultBrush"),
        BorderThickness = new Thickness(1),
    };

    /// <summary>A labelled field (label above the control).</summary>
    public static StackPanel Field(string label, FrameworkElement control)
    {
        control.HorizontalAlignment = HorizontalAlignment.Stretch;
        var panel = new StackPanel { Spacing = 4 };
        panel.Children.Add(new TextBlock { Text = label, Style = Resource<Style>("CaptionTextBlockStyle"), Opacity = 0.8 });
        panel.Children.Add(control);
        return panel;
    }

    /// <summary>An InfoBar that shows the ViewModel's ErrorText (and InfoText) while it is set.</summary>
    public static StackPanel Messages(ResultViewModel viewModel)
    {
        var error = new InfoBar { Severity = InfoBarSeverity.Error, IsClosable = true };
        var info = new InfoBar { Severity = InfoBarSeverity.Success, IsClosable = true };
        Watch(viewModel, nameof(ResultViewModel.ErrorText), () =>
        {
            error.Message = viewModel.ErrorText ?? string.Empty;
            error.IsOpen = viewModel.ErrorText is not null;
        });
        Watch(viewModel, nameof(ResultViewModel.InfoText), () =>
        {
            info.Message = viewModel.InfoText ?? string.Empty;
            info.IsOpen = viewModel.InfoText is not null;
        });
        return Column(error, info);
    }

    /// <summary>A progress ring that is active while the ViewModel is busy.</summary>
    public static ProgressRing Busy(ViewModelAppObject viewModel)
    {
        var ring = new ProgressRing { Width = 20, Height = 20, IsActive = false };
        Watch(viewModel, nameof(ViewModelAppObject.IsBusy), () => ring.IsActive = viewModel.IsBusy);
        return ring;
    }

    /// <summary>Runs <paramref name="update"/> now and whenever the property changes (null = any property).</summary>
    public static void Watch(INotifyPropertyChanged source, string? property, Action update)
    {
        update();
        source.PropertyChanged += (_, e) =>
        {
            if (property is null || string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == property)
            {
                update();
            }
        };
    }

    public static async Task<bool> ConfirmAsync(XamlRoot root, string title, string message, string confirm, string cancel)
    {
        var dialog = new ContentDialog
        {
            XamlRoot = root,
            Title = title,
            Content = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap },
            PrimaryButtonText = confirm,
            CloseButtonText = cancel,
            DefaultButton = ContentDialogButton.Primary,
        };
        return await dialog.ShowAsync() == ContentDialogResult.Primary;
    }

    /// <summary>Shows content in a dialog; <paramref name="accept"/> may refuse to close (e.g. validation failed).</summary>
    public static async Task<bool> DialogAsync(XamlRoot root, string title, UIElement content, string primary, string close, Func<Task<bool>>? accept = null)
    {
        var dialog = new ContentDialog
        {
            XamlRoot = root,
            Title = title,
            Content = new ScrollViewer { Content = content, MaxHeight = 560 },
            PrimaryButtonText = primary,
            CloseButtonText = close,
            DefaultButton = ContentDialogButton.Primary,
        };
        if (accept is not null)
        {
            dialog.PrimaryButtonClick += async (_, args) =>
            {
                var deferral = args.GetDeferral();
                args.Cancel = !await accept();
                deferral.Complete();
            };
        }

        return await dialog.ShowAsync() == ContentDialogResult.Primary;
    }

    public static string Money(decimal amount, string currency) =>
        $"{amount.ToString("N2", System.Globalization.CultureInfo.CurrentCulture)} {currency}";
}
