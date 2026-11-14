// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.WorkflowService
// File    : WorkflowMigration.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Migrations of workflow definitions and running instances between module versions.
//
// SOURCES
//   - Declared by the module in its manifest (e.g. "field 'customer' renamed to 'customerId'",
//     "command documents.send replaced by documents.dispatch").
//   - Generic rules: added optional fields, renamed IDs with an alias, widened types.
//
// BEHAVIOUR
//   - Migrations are data transformations of definitions and variables, tested in the
//     module's CI against sample workflows before the module is published.
