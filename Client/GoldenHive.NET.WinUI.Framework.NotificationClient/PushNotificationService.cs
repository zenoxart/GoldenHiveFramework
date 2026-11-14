// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.NotificationClient
// File    : PushNotificationService.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Push via WNS using PushNotificationManager.
//
// BEHAVIOUR
//   - Request a channel URI and register it with NotificationPoint for the user.
//   - Renew the channel regularly.
//   - WNS is not a guaranteed channel; reliability comes from SignalR plus catch-up.
