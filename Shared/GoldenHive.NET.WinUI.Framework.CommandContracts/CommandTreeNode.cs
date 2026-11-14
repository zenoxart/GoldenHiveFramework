// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.CommandContracts
// File    : CommandTreeNode.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Immutable node of a generated command tree (output of the CommandTreeBuilder).
//
// FIELDS
//   - Node ID (stable: command ID for leaves, placement + path for groups).
//   - Kind: group, command, separator, dynamic placeholder.
//   - Label, icon, tooltip, shortcut text (already resolved for the current language).
//   - For commands: the command ID (adapters resolve it to the PooledCommand).
//   - Effective state at build time: visible, enabled, checked.
//   - Children (ordered).
//
// RULE
//   The node never references module types or handlers - only IDs and display data -
//   so a tree can be kept by the UI across hot-swaps.
