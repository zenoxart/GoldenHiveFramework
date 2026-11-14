using GoldenHive.NET.WinUI.Framework.Mvvm;
using GoldenHive.NET.WinUI.Framework.Security;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;

/// <summary>
/// Sign-in dialog (built in code): server URL, user name, password, "stay signed in". The secondary button continues
/// offline with the locally installed modules.
/// </summary>
internal static class LoginDialog
{
    /// <summary>Shows the dialog until the sign-in succeeds or the user chooses to work offline. Returns true when signed in.</summary>
    public static async Task<bool> ShowAsync(XamlRoot root, ClientAppContext context)
    {
        var languages = context.Languages;
        string T(string key) => languages.GetText("shell:" + key);
        var backend = context.Backend;

        var server = new TextBox { Header = T("login.server"), Text = backend.Settings.ServerUrl };
        var user = new TextBox { Header = T("login.user"), Text = backend.Settings.LastUserName ?? string.Empty };
        var password = new PasswordBox { Header = T("login.password") };
        var remember = new CheckBox { Content = T("login.remember"), IsChecked = backend.Settings.RememberMe };
        var error = new InfoBar { Severity = InfoBarSeverity.Error, IsClosable = false, IsOpen = false };
        var progress = new ProgressBar { IsIndeterminate = true, Visibility = Visibility.Collapsed };
        var hint = new TextBlock { Text = T("login.hint"), TextWrapping = TextWrapping.Wrap, Opacity = 0.7, Style = (Style)Application.Current.Resources["CaptionTextBlockStyle"] };
        var panel = new StackPanel { Spacing = 12, MinWidth = 380 };
        foreach (var element in new UIElement[] { server, user, password, remember, progress, error, hint })
        {
            panel.Children.Add(element);
        }

        var dialog = new ContentDialog
        {
            XamlRoot = root,
            Title = T("login.title"),
            Content = panel,
            PrimaryButtonText = T("login.signin"),
            SecondaryButtonText = T("login.offline"),
            DefaultButton = ContentDialogButton.Primary,
        };
        dialog.Opened += (_, _) => (string.IsNullOrEmpty(user.Text) ? user : (Control)password).Focus(FocusState.Programmatic);
        var signedIn = false;
        dialog.PrimaryButtonClick += async (_, args) =>
        {
            var deferral = args.GetDeferral();
            try
            {
                if (string.IsNullOrWhiteSpace(server.Text) || string.IsNullOrWhiteSpace(user.Text) || string.IsNullOrEmpty(password.Password))
                {
                    error.Message = T("login.required");
                    error.IsOpen = true;
                    args.Cancel = true;
                    return;
                }

                progress.Visibility = Visibility.Visible;
                error.IsOpen = false;
                var result = await backend.SignInAsync(server.Text.Trim(), user.Text.Trim(), password.Password, remember.IsChecked == true);
                progress.Visibility = Visibility.Collapsed;
                if (result.TryGetValue(out var account, out var failure))
                {
                    signedIn = true;
                    return;
                }

                error.Message = ErrorMessages.Format(languages, failure);
                error.IsOpen = true;
                password.Password = string.Empty;
                args.Cancel = true;
            }
            finally
            {
                deferral.Complete();
            }
        };

        await dialog.ShowAsync();
        if (!signedIn)
        {
            context.Log.Write("Working offline (sign-in skipped)", LogEntryType.Warning, nameof(LoginDialog));
        }

        return signedIn;
    }
}
