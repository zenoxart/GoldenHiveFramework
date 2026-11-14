// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.CommandContracts
// File    : ICommandHandler.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Contract for a command's implementation inside a module.
//
// METHODS (described)
//   - CanExecute(parameter)
//   - ExecuteAsync(parameter, CancellationToken)
//   - optional IsChecked(parameter) for toggle and radio commands
//   - optional event when CanExecute / IsChecked changes (the manager then triggers a requery).
//
// DISABLED REASON
//   - optional GetDisabledReason(parameter): returns a reason key (e.g. "noPermission",
//     "nothingSelected") shown in the tooltip when CanExecute is false.
