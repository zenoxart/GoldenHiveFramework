using System.Globalization;
using GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using GoldenHive.NET.WinUI.Framework.Security;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Shell;

/// <summary>
/// The user in the top right corner of the title bar: profile picture (or initials) and name; the flyout shows the
/// profile (user name, e-mail, roles, approval limit, last sign-in, server and connection) with change/remove picture,
/// change password, settings and sign-out. Signed out it offers "Sign in".
/// </summary>
public sealed partial class ProfileButton : UserControl
{
    private const int PictureSize = 256;

    private readonly ClientAppContext _context;
    private readonly Window _window;
    private readonly Func<Task> _signIn;
    private readonly PersonPicture _small = new() { Width = 28, Height = 28 };
    private readonly TextBlock _name = new() { VerticalAlignment = VerticalAlignment.Center, Style = (Style)Application.Current.Resources["CaptionTextBlockStyle"], MaxWidth = 180, TextTrimming = TextTrimming.CharacterEllipsis };
    private readonly Ellipse _status = new() { Width = 9, Height = 9, StrokeThickness = 1.5, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom };
    private readonly Flyout _flyout = new() { Placement = FlyoutPlacementMode.BottomEdgeAlignedRight };
    private ImageSource? _image;

    public ProfileButton(ClientAppContext context, Window window, Func<Task> signIn)
    {
        _context = context;
        _window = window;
        _signIn = signIn;

        var pictureHost = new Grid();
        pictureHost.Children.Add(_small);
        pictureHost.Children.Add(_status);
        var content = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
        content.Children.Add(pictureHost);
        content.Children.Add(_name);

        var button = new Button
        {
            Content = content,
            Padding = new Thickness(6, 2, 8, 2),
            Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
            BorderThickness = new Thickness(0),
            VerticalAlignment = VerticalAlignment.Center,
            Flyout = _flyout,
        };
        _flyout.Opening += (_, _) => _flyout.Content = BuildFlyout();
        Content = button;

        context.Profile.Changed += async (_, _) => await UpdateAsync();
        context.Backend.SignedIn += async (_, _) => await UpdateAsync();
        context.Backend.SignedOut += async (_, _) => await UpdateAsync();
        context.Backend.StateChanged += (_, _) => DispatcherQueue.TryEnqueue(UpdateStatus);
        context.Hub.ConnectionChanged += (_, _) => DispatcherQueue.TryEnqueue(UpdateStatus);
        context.Languages.LanguageChanged += async (_, _) => await UpdateAsync();
        _ = UpdateAsync();
    }

    private string T(string key) => _context.Languages.GetText("shell:" + key);

    private string T(string key, string name, object? value) =>
        _context.Languages.GetText("shell:" + key, new Dictionary<string, object?> { [name] = value });

    private async Task UpdateAsync()
    {
        var user = _context.Backend.CurrentUser;
        _image = await ToImageAsync(_context.Profile.Avatar);
        _small.ProfilePicture = _image;
        _small.DisplayName = user?.DisplayName ?? string.Empty;
        _small.Initials = user is null ? null : UserProfileService.InitialsOf(user.DisplayName);
        _name.Text = user?.DisplayName ?? T("profile.signin");
        ToolTipService.SetToolTip(this, user is null ? T("profile.signin") : $"{user.DisplayName} ({user.UserName})");
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        var backend = _context.Backend;
        var color = !backend.IsSignedIn ? "SystemFillColorNeutralBrush"
            : backend.State == ConnectionState.Offline ? "SystemFillColorCautionBrush"
            : "SystemFillColorSuccessBrush";
        _status.Fill = (Brush)Application.Current.Resources[color];
        _status.Stroke = (Brush)Application.Current.Resources["SolidBackgroundFillColorBaseBrush"];
    }

    private FrameworkElement BuildFlyout()
    {
        var panel = new StackPanel { Spacing = 12, Width = 320 };
        var user = _context.Backend.CurrentUser;
        if (user is null)
        {
            panel.Children.Add(new PersonPicture { Width = 64, Height = 64, HorizontalAlignment = HorizontalAlignment.Center });
            panel.Children.Add(new TextBlock { Text = T("connection.signedout"), HorizontalAlignment = HorizontalAlignment.Center });
            panel.Children.Add(Action(T("profile.signin"), "", async () => await _signIn(), accent: true));
            return panel;
        }

        // Header: big picture (click = change), name, user name and e-mail.
        var picture = new PersonPicture
        {
            Width = 72,
            Height = 72,
            ProfilePicture = _image,
            DisplayName = user.DisplayName,
            Initials = UserProfileService.InitialsOf(user.DisplayName),
        };
        var pictureButton = new Button { Content = picture, Padding = new Thickness(0), CornerRadius = new CornerRadius(40), BorderThickness = new Thickness(0), Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent) };
        ToolTipService.SetToolTip(pictureButton, T("profile.changePicture"));
        pictureButton.Click += async (_, _) => await ChangePictureAsync();
        var names = new StackPanel { Spacing = 2, VerticalAlignment = VerticalAlignment.Center };
        names.Children.Add(new TextBlock { Text = user.DisplayName, Style = (Style)Application.Current.Resources["SubtitleTextBlockStyle"], TextTrimming = TextTrimming.CharacterEllipsis });
        names.Children.Add(Caption("@" + user.UserName));
        names.Children.Add(Caption(user.Email));
        var header = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 14 };
        header.Children.Add(pictureButton);
        header.Children.Add(names);
        panel.Children.Add(header);

        // Facts.
        var facts = new Grid { ColumnSpacing = 12, RowSpacing = 4 };
        facts.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        facts.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        void Fact(string key, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var row = facts.RowDefinitions.Count;
            facts.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var label = Caption(T(key));
            var text = new TextBlock { Text = value, TextWrapping = TextWrapping.Wrap, FontSize = 12 };
            Grid.SetRow(label, row);
            Grid.SetRow(text, row);
            Grid.SetColumn(text, 1);
            facts.Children.Add(label);
            facts.Children.Add(text);
        }

        var backend = _context.Backend;
        Fact("profile.roles", string.Join(", ", user.Roles));
        Fact("profile.limit", user.ApprovalLimit > 0 ? string.Format(CultureInfo.CurrentCulture, "{0:N0} EUR", user.ApprovalLimit) : null);
        Fact("profile.lastLogin", user.LastLoginAt?.ToLocalTime().ToString("g", CultureInfo.CurrentCulture));
        Fact("profile.server", backend.ServerUrl);
        Fact("profile.connection", T(backend.State == ConnectionState.Offline ? "connection.offline" : "connection.online") +
                                    (_context.Hub.IsConnected ? " · " + T("connection.live") : string.Empty));
        Fact("profile.permissions", user.Permissions.Contains("*") ? T("profile.allPermissions") : user.Permissions.Count.ToString(CultureInfo.CurrentCulture));
        panel.Children.Add(facts);

        panel.Children.Add(Separator());
        panel.Children.Add(Action(T("profile.changePicture"), "", ChangePictureAsync));
        if (_context.Profile.Avatar is not null)
        {
            panel.Children.Add(Action(T("profile.removePicture"), "", RemovePictureAsync));
        }

        panel.Children.Add(Action(T("profile.changePassword"), "", ChangePasswordAsync));
        panel.Children.Add(Action(T("view.settings.label"), "", () =>
        {
            _context.Navigation.NavigateTo(BuiltInViews.Settings);
            return Task.CompletedTask;
        }));
        panel.Children.Add(Separator());
        panel.Children.Add(Action(T("command.signout.label"), "", () =>
        {
            backend.SignOut();
            return Task.CompletedTask;
        }));
        return panel;
    }

    private static Border Separator() =>
        new() { Height = 1, Margin = new Thickness(0, 2, 0, 2), Background = (Brush)Application.Current.Resources["DividerStrokeColorDefaultBrush"] };

    private static TextBlock Caption(string text) =>
        new() { Text = text, Style = (Style)Application.Current.Resources["CaptionTextBlockStyle"], Opacity = 0.75, TextTrimming = TextTrimming.CharacterEllipsis };

    private Button Action(string text, string glyph, Func<Task> action, bool accent = false)
    {
        var content = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 12 };
        content.Children.Add(new FontIcon { Glyph = glyph, FontSize = 16 });
        content.Children.Add(new TextBlock { Text = text });
        var button = new Button
        {
            Content = content,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Left,
            Padding = new Thickness(10, 6, 10, 6),
        };
        if (accent)
        {
            button.Style = (Style)Application.Current.Resources["AccentButtonStyle"];
        }
        else
        {
            button.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            button.BorderThickness = new Thickness(0);
        }

        button.Click += async (_, _) =>
        {
            _flyout.Hide();
            await action();
        };
        return button;
    }

    // ------------------------------------------------------------------ picture

    private async Task ChangePictureAsync()
    {
        _flyout.Hide();
        var picker = new FileOpenPicker { ViewMode = PickerViewMode.Thumbnail, SuggestedStartLocation = PickerLocationId.PicturesLibrary };
        foreach (var extension in new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp" })
        {
            picker.FileTypeFilter.Add(extension);
        }

        WinRT.Interop.InitializeWithWindow.Initialize(picker, WinRT.Interop.WindowNative.GetWindowHandle(_window));
        var file = await picker.PickSingleFileAsync();
        if (file is null)
        {
            return;
        }

        byte[] png;
        try
        {
            using var stream = await file.OpenReadAsync();
            png = await SquarePngAsync(stream);
        }
        catch (Exception ex)
        {
            _context.Log.Write($"Profile picture could not be read: {ex.Message}", LogEntryType.Warning, nameof(ProfileButton), ex);
            await ShowErrorAsync(T("profile.pictureUnreadable"));
            return;
        }

        var result = await _context.Profile.SetAvatarAsync(png);
        if (result.IsFailure)
        {
            await ShowErrorAsync(ErrorMessages.Format(_context.Languages, result.Error));
        }
    }

    private async Task RemovePictureAsync()
    {
        var result = await _context.Profile.RemoveAvatarAsync();
        if (result.IsFailure)
        {
            await ShowErrorAsync(ErrorMessages.Format(_context.Languages, result.Error));
        }
    }

    /// <summary>Crops the image to the centred square, scales it to at most 256x256 and encodes it as PNG.</summary>
    private static async Task<byte[]> SquarePngAsync(IRandomAccessStream source)
    {
        var decoder = await BitmapDecoder.CreateAsync(source);
        var width = decoder.OrientedPixelWidth;
        var height = decoder.OrientedPixelHeight;
        var scale = Math.Min(1.0, (double)PictureSize / Math.Min(width, height));
        var scaledWidth = (uint)Math.Max(1, Math.Round(width * scale));
        var scaledHeight = (uint)Math.Max(1, Math.Round(height * scale));
        var side = Math.Min(scaledWidth, scaledHeight);
        var transform = new BitmapTransform
        {
            ScaledWidth = scaledWidth,
            ScaledHeight = scaledHeight,
            InterpolationMode = BitmapInterpolationMode.Fant,
            Bounds = new BitmapBounds { X = (scaledWidth - side) / 2, Y = (scaledHeight - side) / 2, Width = side, Height = side },
        };
        var pixels = await decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, transform,
            ExifOrientationMode.RespectExifOrientation, ColorManagementMode.DoNotColorManage);

        using var output = new InMemoryRandomAccessStream();
        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, output);
        encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, side, side, 96, 96, pixels.DetachPixelData());
        await encoder.FlushAsync();

        var bytes = new byte[output.Size];
        using var reader = new DataReader(output.GetInputStreamAt(0));
        await reader.LoadAsync((uint)output.Size);
        reader.ReadBytes(bytes);
        return bytes;
    }

    private static async Task<ImageSource?> ToImageAsync(byte[]? bytes)
    {
        if (bytes is null || bytes.Length == 0)
        {
            return null;
        }

        try
        {
            using var stream = new InMemoryRandomAccessStream();
            using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
            {
                writer.WriteBytes(bytes);
                await writer.StoreAsync();
                await writer.FlushAsync();
                writer.DetachStream();
            }

            stream.Seek(0);
            var image = new BitmapImage { DecodePixelWidth = 144, DecodePixelType = DecodePixelType.Logical };
            await image.SetSourceAsync(stream);
            return image;
        }
        catch (Exception)
        {
            return null;
        }
    }

    // ------------------------------------------------------------------ password

    private async Task ChangePasswordAsync()
    {
        var current = new PasswordBox { Header = T("profile.currentPassword") };
        var next = new PasswordBox { Header = T("profile.newPassword") };
        var repeat = new PasswordBox { Header = T("profile.repeatPassword") };
        var error = new InfoBar { Severity = InfoBarSeverity.Error, IsClosable = false, IsOpen = false };
        var panel = new StackPanel { Spacing = 12, MinWidth = 340 };
        panel.Children.Add(current);
        panel.Children.Add(next);
        panel.Children.Add(repeat);
        panel.Children.Add(new TextBlock { Text = T("profile.passwordHint"), Opacity = 0.7, TextWrapping = TextWrapping.Wrap, Style = (Style)Application.Current.Resources["CaptionTextBlockStyle"] });
        panel.Children.Add(error);

        var dialog = new ContentDialog
        {
            XamlRoot = XamlRoot,
            Title = T("profile.changePassword"),
            Content = panel,
            PrimaryButtonText = T("dialog.save"),
            CloseButtonText = T("dialog.cancel"),
            DefaultButton = ContentDialogButton.Primary,
        };
        dialog.PrimaryButtonClick += async (_, args) =>
        {
            var deferral = args.GetDeferral();
            try
            {
                if (next.Password != repeat.Password)
                {
                    error.Message = T("profile.passwordMismatch");
                    error.IsOpen = true;
                    args.Cancel = true;
                    return;
                }

                var result = await _context.Profile.ChangePasswordAsync(current.Password, next.Password);
                if (result.IsFailure)
                {
                    var fields = ErrorMessages.Fields(_context.Languages, result.Error);
                    error.Message = fields.Count > 0 ? string.Join("\n", fields.Values) : ErrorMessages.Format(_context.Languages, result.Error);
                    error.IsOpen = true;
                    args.Cancel = true;
                    return;
                }

                _context.InApp.Show("shell:profile.passwordChanged", null, GoldenHive.NET.WinUI.Framework.NotificationContracts.NotificationSeverity.Success);
            }
            finally
            {
                deferral.Complete();
            }
        };
        await dialog.ShowAsync();
    }

    private async Task ShowErrorAsync(string message)
    {
        var dialog = new ContentDialog
        {
            XamlRoot = XamlRoot,
            Title = T("profile.title"),
            Content = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap },
            CloseButtonText = T("about.close"),
        };
        await dialog.ShowAsync();
    }
}
