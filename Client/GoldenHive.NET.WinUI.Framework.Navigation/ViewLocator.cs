// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Navigation
// File    : ViewLocator.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Finds the matching view for a ViewModel.
//
// BEHAVIOUR
//   - The mapping comes from the NavigationRegistry (not a naming convention on type names,
//     since types come from different load contexts).
//   - Creation via LooseXamlViewFactory or a module factory, depending on viewMode.
