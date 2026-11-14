// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Security
// File    : PermissionService.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Client-side gating of features based on the user's permissions.
//
// RULES
//   - For the UI only (hide/disable menus and buttons).
//   - Every protected operation is authorized again on the server.
//   - On a permission change (event from NotificationPoint) call RelayCommandPool.RequeryAll
//     and rebuild the menu.
