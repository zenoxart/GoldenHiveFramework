// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.NotificationClient
// File    : NotificationHubClient.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   SignalR connection to NotificationPoint.
//
// BEHAVIOUR
//   - Connect with the access token from AuthenticationService.
//   - Automatic reconnect; after reconnecting, fetch missed events from the
//     last acknowledged sequence number.
//   - Deduplicate by event ID.
//   - Pass received events to the NotificationRouter.
