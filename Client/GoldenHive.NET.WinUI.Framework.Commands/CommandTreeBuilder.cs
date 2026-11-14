// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : CommandTreeBuilder.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Generates hierarchical command trees from the flat list of registered commands,
//   for tree menus (menu bar, nested context menus, TreeView side panels,
//   hierarchical NavigationView).
//
// INPUT
//   - All registered CommandDescriptors and CommandGroupDescriptors.
//   - The requested placement (e.g. "shell.mainmenu", "shell.navigation",
//     "context.document", "panel.tools").
//   - A CommandTreeContext: current user permissions, active module, selected item,
//     window, and an optional filter text.
//
// ALGORITHM
//   1. Collect every descriptor that has a placement entry for the requested placement.
//   2. Resolve each entry's path (e.g. "file/export") into group nodes.
//      Missing intermediate groups are created implicitly; their label comes from a
//      registered group descriptor if one exists, otherwise from the path segment.
//   3. Merge contributions from several modules into the same group
//      (e.g. host and DocumentManagement both add items under "file").
//   4. Apply visibility rules per node: hide when the user lacks the permission,
//      or disable instead, depending on the descriptor's visibility policy.
//   5. Sort siblings by section, then order, then label.
//   6. Insert separators between different sections inside the same group.
//   7. Expand dynamic nodes by calling their ICommandTreeProvider (e.g. "recent documents").
//   8. Remove empty groups and leading/trailing/duplicate separators.
//   9. Return an immutable CommandTree with a version stamp.
//
// CACHING
//   - Cache per placement + permission set + active module.
//   - Invalidate on registry change, permission change or hot-swap; then raise TreeChanged.
//   - Dynamic providers are evaluated lazily when a submenu opens, not on every rebuild.
//
// RULES
//   - Node IDs are stable (command ID or group path), so adapters can diff old and new
//     trees and keep expansion/selection state.
//   - Leaves always point to the PooledCommand of their command ID, never to a handler.
//   - Detect cycles and a configurable maximum depth; report violations at registration time.
