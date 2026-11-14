// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.NotificationPoint
// File    : Outbox.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Reliable delivery.
//
// BEHAVIOUR
//   - Outbox pattern in the Docker modules: store the event in the same transaction as
//     the domain data, publish it afterwards.
//   - Keep events for a retention period (for catch-up).
