// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : CommandExecutor.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Runs commands in a uniform way.
//
// FLOW
//   1. Check permission via PermissionService (UI gating only; the server checks again).
//   2. Register the running execution with the hot-swap manager (ModuleQuiescence).
//   3. Run the handler asynchronously with a CancellationToken.
//   4. Catch errors, log them, show them as an in-app notification and
//      report them to NotificationPoint.
//   5. Unregister the execution; trigger a requery if needed.
//
// RULES
//   - Optionally block concurrent execution of the same command (IsExecuting).
//   - Long-running operations report progress via the NotificationClient.
