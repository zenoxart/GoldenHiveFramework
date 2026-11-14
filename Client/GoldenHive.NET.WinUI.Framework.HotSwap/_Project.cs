// GoldenHive.NET.WinUI.Framework.HotSwap
//
// Swaps running client modules when the Update Orchestrator assigns a new version:
//   HotSwapManager    listens to module.version.available / module.update.status, decides per module (hotSwap mode,
//                     user choice) and lets the ModuleLoader replace the module.
//   ModuleQuiescence  counts running commands per module, blocks new ones during a swap and cancels stragglers.
//   UnloadMonitor     tracks the replaced load contexts (best-effort: contexts are not collectible because WinUI types
//                     cannot be unloaded reliably; the monitor reports what stays in memory).
// Principle: activate the new version reliably; the old version is released best-effort.
