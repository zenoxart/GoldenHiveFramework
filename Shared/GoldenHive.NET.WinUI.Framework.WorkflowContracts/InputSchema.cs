// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.WorkflowContracts
// File    : InputSchema.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Describes the inputs of an Input step, so the client can generate the form and
//   the AI agent knows exactly what to provide.
//
// FIELDS
//   - SchemaId + Version (owned by a module; part of the module's contract surface).
//   - Fields: name, type (text, number, date, boolean, choice, entity reference via module API,
//     file, multi-line), label/help resource keys, default value, required flag.
//   - Validation rules: ranges, patterns, cross-field rules, server-side validation endpoint.
//   - Sensitive flag per field (masked in logs and in AI context).
//
// RULE
//   Changing a field's type, removing a field or making a field required is a breaking change
//   and bumps the major version; adding an optional field is compatible.
//
// LAYOUT HINTS
//   Optional per field: span (full width / half), group; the presenter maps them to the
//   current size class - schemas never contain pixel sizes.
