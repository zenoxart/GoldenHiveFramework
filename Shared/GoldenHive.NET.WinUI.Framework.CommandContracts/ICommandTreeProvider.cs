// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.CommandContracts
// File    : ICommandTreeProvider.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Supplies dynamic children for a group at the moment it opens.
//
// EXAMPLES
//   Recent documents, open windows, available report templates, plugins of a module.
//
// METHODS (described)
//   - GetChildrenAsync(context, CancellationToken): returns descriptors for the
//     current children; each child carries a command ID plus its parameter.
//
// RULES
//   - Must be fast; long work is cancelled when the menu closes.
//   - Removed automatically when the owning module unloads.
