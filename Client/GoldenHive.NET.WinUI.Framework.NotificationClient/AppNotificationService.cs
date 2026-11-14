using GoldenHive.NET.WinUI.Framework.Mvvm;
using GoldenHive.NET.WinUI.Framework.NotificationContracts;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

namespace GoldenHive.NET.WinUI.Framework.NotificationClient;

/// <summary>
/// Windows toast notifications (Windows App SDK) for notifications that arrive while the app is not in the foreground.
/// Clicking a toast activates the app and navigates to the notification's target.
/// </summary>
public sealed class AppNotificationService : AppObject
{
    private bool _registered;

    /// <summary>Called with (navigation target, parameter) when a toast is clicked.</summary>
    public Action<string, string?>? Navigate { get; set; }

    /// <summary>Returns true while the main window is in the foreground (then no toast is shown).</summary>
    public Func<bool>? IsForeground { get; set; }

    public void Register()
    {
        try
        {
            AppNotificationManager.Default.NotificationInvoked += OnInvoked;
            AppNotificationManager.Default.Register();
            _registered = true;
        }
        catch (Exception ex)
        {
            AppContext.Log.Write($"Windows notifications are not available: {ex.Message}", LogEntryType.Warning, nameof(AppNotificationService));
        }
    }

    public void Unregister()
    {
        if (!_registered)
        {
            return;
        }

        try
        {
            AppNotificationManager.Default.Unregister();
        }
        catch (Exception ex)
        {
            AppContext.Log.Write($"Unregistering Windows notifications failed: {ex.Message}", LogEntryType.Warning, nameof(AppNotificationService));
        }
    }

    public void Show(InAppNotification notification)
    {
        if (!_registered || IsForeground?.Invoke() == true || notification.Severity == NotificationSeverity.Info && notification.NavigationTarget is null)
        {
            return;
        }

        try
        {
            var builder = new AppNotificationBuilder().AddText(notification.Title);
            if (!string.IsNullOrWhiteSpace(notification.Message))
            {
                builder.AddText(notification.Message);
            }

            if (notification.NavigationTarget is { } target)
            {
                builder.AddArgument("navigate", target);
                if (notification.NavigationParameter is { } parameter)
                {
                    builder.AddArgument("parameter", parameter);
                }
            }

            AppNotificationManager.Default.Show(builder.BuildNotification());
        }
        catch (Exception ex)
        {
            AppContext.Log.Write($"Toast could not be shown: {ex.Message}", LogEntryType.Warning, nameof(AppNotificationService));
        }
    }

    private void OnInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
    {
        if (args.Arguments.TryGetValue("navigate", out var target))
        {
            args.Arguments.TryGetValue("parameter", out var parameter);
            AppContext.Dispatcher?.Run(() => Navigate?.Invoke(target, parameter));
        }
    }
}
