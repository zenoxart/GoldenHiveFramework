// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : CommandTreeAdapters.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Projects a CommandTree onto WinUI controls.
//
// TARGETS
//   - MenuBar: root groups -> MenuBarItem; groups -> MenuFlyoutSubItem;
//     commands -> MenuFlyoutItem / ToggleMenuFlyoutItem / RadioMenuFlyoutItem;
//     separators -> MenuFlyoutSeparator.
//   - MenuFlyout (context menus): same mapping, built when the flyout opens.
//   - TreeView (tree menus in side panels): groups -> expandable TreeViewNodes,
//     commands -> leaf nodes; invoking a leaf executes its command.
//   - NavigationView: groups -> NavigationViewItem with MenuItems (hierarchical navigation),
//     commands -> items whose invocation runs the (navigation) command.
//   - CommandBar: flat projection of one level (primary vs. secondary by descriptor).
//
// BEHAVIOUR
//   - Leaf items bind to the stable PooledCommand / XamlUICommand, so enabled state and
//     labels update automatically and survive a hot-swap without rebuilding the menu.
//   - On TreeChanged: diff old and new tree by node ID and patch the control instead of
//     recreating it; keep TreeView expansion and NavigationView selection.
//   - Icons, labels, shortcuts and tooltips come from CommandUiDescriptor.
//   - Keyboard access keys and accelerators are applied from the descriptor.
//
// RADIAL WHEEL
//   - NavigationWheel: root nodes -> inner ring segments, a node's children -> outer ring
//     fan-out; badges and icons from the node; selection executes the node's command.
//
// ICONS AND TOOLTIPS
//   Every generated menu item, tree node, wheel segment and command bar button gets its icon,
//   localized label and rich tooltip from the CommandUiDescriptor - menus need no extra code.
