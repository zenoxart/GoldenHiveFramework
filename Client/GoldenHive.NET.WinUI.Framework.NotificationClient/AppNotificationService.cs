// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.NotificationClient
// File    : AppNotificationService.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Windows toasts via AppNotificationManager (Windows App SDK).
//
// BEHAVIOUR
//   - Register at startup; pass NotificationInvoked on to the ActivationHandler.
//   - Toast arguments: module ID, navigation key, parameters.
//   - Only show when the app is not in the foreground (otherwise use an InfoBar).
