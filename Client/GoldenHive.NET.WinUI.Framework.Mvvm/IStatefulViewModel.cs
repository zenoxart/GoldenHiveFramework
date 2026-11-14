// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : IStatefulViewModel.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Contract for ViewModelAppObjects whose state must survive a hot-swap.
//
// FLOW
//   - Before the swap: return the state as a serializable object
//     (simple types / DTOs from Contracts only - never types from the old module!).
//   - After the swap: the new ViewModel, produced by the new ModuleAppContext,
//     receives the state back.
//   - Include a version field in the state so the new module version recognises old formats.
