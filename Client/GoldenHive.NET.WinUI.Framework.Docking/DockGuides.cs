// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Docking
// File    : DockGuides.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The docking guide overlay shown while dragging (Visual Studio "diamond").
//
// ELEMENTS
//   - Centre diamond over the hovered tab group / pane: dock as tab (centre), split left/right/
//     top/bottom of it.
//   - Edge guides of the window: dock to the outer left/right/top/bottom of the whole layout.
//   - Semi-transparent preview of the target area; Golden Hive accent colours; high-contrast aware.
