// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.WorkflowContracts
// File    : WorkflowTransition.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Connects two steps.
//
// FIELDS
//   - FromStepId, ToStepId (or End).
//   - Condition: WorkflowExpression over variables and the last step's result;
//     no condition = default transition.
//   - Order: evaluated in order, first match wins.
