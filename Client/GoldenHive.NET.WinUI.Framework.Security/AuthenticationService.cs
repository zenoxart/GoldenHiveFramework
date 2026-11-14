// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Security
// File    : AuthenticationService.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Sign-in against the Identity Service.
//
// BEHAVIOUR
//   - MSAL with the WAM broker (Windows account, SSO); silent token renewal.
//   - Pass the main window handle to the broker dialog.
//   - Event on sign-out / user switch -> unload modules, reset the shell.
