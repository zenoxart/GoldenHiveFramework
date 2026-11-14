// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.NotificationClient
// File    : NotificationRouter.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Dispatches incoming events.
//
// TARGETS
//   - System events (new module version, maintenance) -> HotSwapManager or shell.
//   - Domain events (e.g. DocumentCreated) -> subscribed modules.
//   - User notifications -> InAppNotificationService; in the background -> AppNotificationService.
//
// RULE
//   Module subscriptions are removed automatically when a module is unloaded.
