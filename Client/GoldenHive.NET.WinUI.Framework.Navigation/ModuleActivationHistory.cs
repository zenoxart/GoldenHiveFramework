// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Navigation
// File    : ModuleActivationHistory.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Most-recently-used order of modules and navigation targets, for the navigation wheel.
//
// BEHAVIOUR
//   - The NavigationService records every navigation (module ID + target key + time).
//   - Per window; the order survives restarts (stored with JsonController in LocalDataPath).
//   - Modules that are no longer loaded or permitted are skipped but kept in the history,
//     so they return to their position after a hot-swap.
