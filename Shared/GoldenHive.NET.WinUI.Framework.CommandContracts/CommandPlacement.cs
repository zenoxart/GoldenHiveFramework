// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.CommandContracts
// File    : CommandPlacement.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Where a command or group appears in a command tree.
//
// FIELDS
//   - Placement: name of the tree/menu (e.g. shell.mainmenu, shell.navigation,
//     context.document, panel.tools). Modules may define their own placements.
//   - Path: parent group path inside that tree (e.g. "file/export"); empty = root level.
//   - Section: logical block inside the parent; separators are drawn between sections.
//   - Order: position within the section.
//
// CONVENTIONS
//   - Placement and path segments are lowercase, dot/slash separated, stable across versions.
//   - Host-defined top-level groups (file, edit, view, tools, help) are documented here
//     so modules attach to them consistently.
//
// SHELL PLACEMENTS
//   shell.mainmenu, shell.navigation, shell.wheel (navigation wheel: root = modules,
//   shell.wheel/<moduleId> = a module's outer ring), context.<entity>, panel.<name>.
