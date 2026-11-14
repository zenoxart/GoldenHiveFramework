// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.CommandContracts
// File    : CommandGroupDescriptor.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Describes a group node (submenu / tree folder) explicitly.
//
// FIELDS
//   - Path within a placement (e.g. "file/export"), label resource key, icon.
//   - Order and section within its parent.
//   - Required permission (hides the whole subtree).
//   - Optional ICommandTreeProvider for dynamic children.
//
// NOTE
//   Groups do not have to be registered: the CommandTreeBuilder creates missing groups
//   implicitly. Registering one only adds a label, icon, order or permission.
