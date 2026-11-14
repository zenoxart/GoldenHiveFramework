// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.WorkflowContracts
// File    : WorkflowExpression.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   A small, safe expression language for mappings and conditions.
//
// RULES
//   - Read variables, step results and trigger payload; compare, combine, format, simple math.
//   - No method calls, no reflection, no I/O - expressions are data, not code.
//   - Parsed and type-checked when a definition is saved, so errors appear in the designer
//     and in the impact analysis, not at run time.
