// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.NotificationContracts
// File    : NotificationEnvelope.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Envelope of every notification.
//
// FIELDS
//   Event ID (deduplication), sequence number (catch-up), type, source (module/service),
//   timestamp, tenant, audience (user, role, everyone), severity, payload (JSON),
//   optional navigation target for toast activation.
