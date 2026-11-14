// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Docking
// File    : DockDragService.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Drag and drop of tabs and tool windows.
//
// BEHAVIOUR
//   - Dragging a tab or tool window header starts a drag; leaving the tab strip tears it off into
//     a FloatingDockWindow that follows the pointer.
//   - While dragging over a dock target, DockGuides are shown; dropping on a guide docks there
//     (with a preview rectangle of the resulting area); Esc cancels and restores the old position.
//   - Holding Ctrl while dragging suppresses docking (just move the floating window), like VS.
//   - Works across windows and monitors (drag from a floating window back into the main window).
//   - Keyboard alternative for everything: tab context menu / Alt+- menu.
