// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Workflows
// File    : WorkflowRecorder.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Creates a draft workflow by recording what the user does.
//
// BEHAVIOUR
//   - While recording, listens to the central command manager: every executed
//     workflow-capable command and the inputs entered in schema-based forms become steps.
//   - Values typed by the user become parameters or defaults; the user decides in the
//     designer which values become workflow inputs.
//   - Sensitive fields are never recorded as default values.
