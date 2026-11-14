// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Docking
// File    : DockHost.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The control that renders a DockLayoutModel in a window (built in code).
//
// RENDERING
//   - SplitNodes as Grids with GridSplitter-style resize handles (keyboard-resizable too:
//     focus a splitter, arrow keys move it).
//   - Tab groups as TabView (tab strip, overflow dropdown, close buttons, middle-click close,
//     pinned tabs, tab context menu: float, dock, new horizontal/vertical tab group, close others).
//   - Tool panes with a header (title, pin/auto-hide, menu, close) and bottom tabs when several
//     tool windows share a pane.
//   - Active element highlighted with the Golden Hive accent (like VS's blue active tab).
//
// RESPONSIVE BEHAVIOUR (does not change the saved layout)
//   - Expanded/Wide : full layout as saved.
//   - Medium        : left and right tool panes become auto-hide; bottom pane stays docked.
//   - Compact       : one document at a time with a tab dropdown; all tool windows become
//                     auto-hide flyouts opened from a compact side strip; splits are flattened.
//   When the window grows again, the saved layout returns unchanged.
//
// ICONS AND TOOLTIPS
//   Tabs and tool window headers show the view's icon and localized title; the tab tooltip shows
//   the full title and the module name.
