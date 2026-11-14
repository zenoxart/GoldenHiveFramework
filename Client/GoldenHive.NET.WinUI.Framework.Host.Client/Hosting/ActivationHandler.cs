// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Host.Client
// File    : ActivationHandler.cs
// Layer   : Client / Host
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Handles activations that do not come from a normal launch.
//
// SOURCES
//   - AppNotification (toast) clicked -> arguments contain module ID + target + parameters.
//   - PushNotification (WNS) in the background -> show a toast or prefetch data if needed.
//   - Protocol activation (e.g. zenox://documents/42) -> deep link into a module.
//   - Redirected activation from a second instance.
//
// BEHAVIOUR
//   - Resolve the target via NavigationRegistry; if the module is not loaded yet,
//     queue the activation and run it once the module has loaded.
//   - Check permission via PermissionService before navigating.
