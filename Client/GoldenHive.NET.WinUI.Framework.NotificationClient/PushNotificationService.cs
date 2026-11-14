using System.Text;
using System.Text.Json;
using GoldenHive.NET.WinUI.Framework.Contracts;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using GoldenHive.NET.WinUI.Framework.NotificationContracts;
using GoldenHive.NET.WinUI.Framework.Security;
using Microsoft.Windows.PushNotifications;

namespace GoldenHive.NET.WinUI.Framework.NotificationClient;

/// <summary>
/// Optional Windows push (WNS): registers a push channel and sends its URI to the NotificationPoint, which pushes events
/// for this user while no live connection exists. Requires the Azure app ID of the WNS registration (RemoteId); without
/// it the service stays inactive and events are delivered by catch-up when the app reconnects.
/// </summary>
public sealed class PushNotificationService : AppObject
{
    public Guid? RemoteId { get; set; }

    public async Task RegisterAsync()
    {
        if (RemoteId is not { } remoteId)
        {
            return;
        }

        try
        {
            if (!PushNotificationManager.IsSupported())
            {
                return;
            }

            PushNotificationManager.Default.PushReceived += (_, args) =>
            {
                var json = Encoding.UTF8.GetString(args.Payload);
                if (JsonSerializer.Deserialize<NotificationEnvelope>(json, GoldenHiveJson.Options) is { } envelope)
                {
                    AppContext.Dispatcher?.Run(() => Context<INotificationContext>().Router.Publish(envelope));
                }
            };
            PushNotificationManager.Default.Register();
            var channel = await PushNotificationManager.Default.CreateChannelAsync(remoteId);
            if (channel.Status == PushNotificationChannelStatus.CompletedSuccess)
            {
                var registered = await Context<ISecurityContext>().Backend.PostAsync<JsonElement?>("/api/notifications/channels",
                    new { channelUri = channel.Channel.Uri.ToString() });
                AppContext.Log.Write(registered.IsSuccess ? "Push channel registered" : $"Push channel registration failed: {registered.Error}",
                    registered.IsSuccess ? LogEntryType.Information : LogEntryType.Warning, nameof(PushNotificationService));
            }
        }
        catch (Exception ex)
        {
            AppContext.Log.Write($"Push notifications are not available: {ex.Message}", LogEntryType.Warning, nameof(PushNotificationService));
        }
    }
}
