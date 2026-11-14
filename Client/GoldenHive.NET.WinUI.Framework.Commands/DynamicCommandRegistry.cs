// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : DynamicCommandRegistry.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Maps command IDs to the currently valid implementation (ICommandHandler).
//
// RESPONSIBILITIES
//   - Register(moduleId, descriptor, handler) when a module loads.
//   - Replace(moduleId, ...) during a hot-swap: switch all IDs of a module atomically.
//   - Unregister(moduleId) on unload; afterwards CanExecute = false.
//   - Detect conflicts: two modules registering the same ID -> error, never overwrite.
//   - Thread-safe, since loading happens in the background and queries on the UI thread.
