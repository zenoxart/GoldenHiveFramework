// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.NotificationPoint
// File    : NotificationHub.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   SignalR hub for the WinUI clients.
//
// BEHAVIOUR
//   - Groups per user, role and tenant.
//   - Send only authorized events to clients.
//   - Catch-up: the client reports its last sequence number, the hub sends what is missing from storage.
