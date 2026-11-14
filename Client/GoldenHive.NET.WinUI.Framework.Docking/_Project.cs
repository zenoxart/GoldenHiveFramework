// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Docking
// File    : _Project.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PROJECT
//   WinUI class library: a self-written dock layout similar to Visual Studio Community.
//   WinUI 3 has no built-in docking control, so this library provides it.
//   Builds on Mvvm, Commands and Windowing.
//
// LAYOUT (main window)
//   +---------------------------------------------------------------+
//   | Title bar (search, account)      | Menu bar | Toolbar           |
//   +--+---------------+-----------------------------+-------------+--+
//   |A | Tool window   |  Document well              | Tool window |A |
//   |u | (left, e.g.   |  [Tab][Tab][Tab]   | [Tab]  | (right, e.g.|u |
//   |t |  Module       |                    |        |  Properties)|t |
//   |o |  Explorer)    |  tab group 1       | group 2|             |o |
//   |  |               +--------------------+--------+             |  |
//   |h |               | Tool windows (bottom, tabbed: Output,     |h |
//   |i |               | Notifications, Workflow inbox)            |i |
//   |d +---------------+-------------------------------------------+d |
//   |e   auto-hide tab strips on all four edges                      e|
//   +---------------------------------------------------------------+
//   | Status bar                                                      |
//   +---------------------------------------------------------------+
//
// CONTENTS
//   DockManager, DockLayoutModel, DockHost, DocumentWell, ToolWindowPane, AutoHideManager,
//   DockDragService, DockGuides, FloatingDockWindow, DockLayoutStore.
