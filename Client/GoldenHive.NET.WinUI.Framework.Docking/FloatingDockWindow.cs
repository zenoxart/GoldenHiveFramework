// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Docking
// File    : FloatingDockWindow.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   A floating window of the dock layout (the detached view window of the architecture).
//
// BEHAVIOUR
//   - Can contain one view or a full layout of its own (several tab groups / tool windows),
//     like a floating Visual Studio window.
//   - Created and restored through the Windowing DetachedViewManager / WindowPlacementStore, so
//     its monitor and position are saved in AppData and recalled on the next start.
//   - Title bar actions: dock back (to its last docked position), always on top, close.
//   - Own ResponsiveLayoutManager, navigation wheel and shortcut routing.
