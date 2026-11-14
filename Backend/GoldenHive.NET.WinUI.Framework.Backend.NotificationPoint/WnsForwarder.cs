// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.NotificationPoint
// File    : WnsForwarder.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Forwards selected events to WNS as push notifications.
//
// BEHAVIOUR
//   - Store the clients' channel URIs per user (reported by the PushNotificationService).
//   - Only for users without an active SignalR connection.
//   - Remove expired channels.
