// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Docking
// File    : DockLayoutModel.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Serializable tree describing a dock layout (independent of WinUI controls).
//
// NODES
//   - LayoutRoot      : per window; contains the main split, auto-hide sides, floating windows.
//   - SplitNode       : horizontal or vertical split with ordered children and star sizes
//                       (proportions, so the layout scales when the window is resized).
//   - DocumentWell    : exactly one per main window; contains TabGroups (split like SplitNode).
//   - TabGroup        : ordered tabs + selected tab; pinned tabs first (pin like in VS).
//   - ToolPane        : a group of tool windows docked together (tabs at the bottom of the pane).
//   - AutoHideSide    : Left/Right/Top/Bottom strips with collapsed tool windows and their
//                       flyout sizes.
//   - FloatingWindow  : reference to a floating window: its own LayoutRoot + window placement.
//   - ViewRef         : view key (navigation key + serialized parameter), module ID, role,
//                       title cache, and placeholder flag when the module is not available.
//
// RULES
//   - Sizes are stored as proportions plus minimum sizes, never as absolute pixels, so a layout
//     adapts to any window size (responsive first).
//   - Version number on the model for migrations of saved layouts.
