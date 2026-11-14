// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : PooledCommand.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The ICommand implementation that WinUI controls bind to.
//
// BEHAVIOUR
//   - Holds only the command ID and a reference to the pool (registry + dispatcher),
//     never a reference to module types (otherwise the old context cannot be unloaded).
//   - CanExecute: ask the registry for the current implementation; none -> false.
//   - Execute: delegate to CommandExecutor (asynchronously).
//   - Raise CanExecuteChanged only through the pool's CommandDispatcher (UI thread).
