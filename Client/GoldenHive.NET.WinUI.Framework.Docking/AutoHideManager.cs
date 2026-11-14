// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Docking
// File    : AutoHideManager.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Auto-hide (unpinned) tool windows like Visual Studio.
//
// BEHAVIOUR
//   - Collapsed tool windows show as vertical/horizontal tabs on the window edge.
//   - Hover (with a short delay) or click opens the tool window as a flyout over the documents;
//     it closes when focus moves elsewhere; pin docks it again.
//   - Flyout size per tool window is stored; keyboard: Alt+Shift+<tool shortcut> opens it.
