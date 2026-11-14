// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.HotSwap
// File    : HotSwapManager.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Controls swapping a module at runtime.
//
// SEQUENCE
//   1. Receive the update notice from the NotificationClient / Update Orchestrator.
//   2. Check the manifest: hotSwap = restartRequired -> only show "restart required".
//   3. Download and validate the new version (ModuleLoader), load it into a new context.
//   4. Ask the user if needed, or wait for a quiet moment.
//   5. ModuleQuiescence: wait for or cancel the module's running commands.
//   6. Save the state of the module's ViewModels (IStatefulViewModel).
//   7. Close module pages, clean the Frame back stack, detach event handlers and subscriptions.
//   8. Switch registry, navigation and metadata to the new version; requery.
//   9. Rebuild the views, restore state, restore the active page.
//  10. Hand the old context over to the UnloadMonitor.
//
// ROLLBACK
//   If step 8 or 9 fails, switch back to the old version and report the error.
//
// WORKFLOWS
//   - Before step 5 (quiescence): raise the ModuleSwapping event; the WorkflowManager subscribes
//     (HotSwap does not reference Workflows) and brings running workflow
//     instances of the module to a step boundary, persists and pauses them.
//   - After the swap (ModuleSwapped event): the WorkflowManager continues, migrates or keeps them paused according
//     to the module update's workflow impact report.
