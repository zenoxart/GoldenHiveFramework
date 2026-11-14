// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Workflows
// File    : WorkflowDesignerViewModel.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   In-app designer to define workflows over several input interactions inside a module.
//   A ViewModelAppObject.
//
// FEATURES
//   - Building blocks palette: module step types, input schemas and commands marked as
//     workflow-capable - grouped by module (taken from the command manager's trees).
//   - Canvas or ordered step list with transitions and conditions; variable panel;
//     expression editor with live type checking.
//   - Triggers: manual, command, event, schedule, condition - each with an executor
//     (human or AI) and a RunAs user.
//   - AI policy editor (autonomy level, allowed steps, approvers, limits, data scope).
//   - Dependencies preview: what the workflow relies on, computed live.
//   - Test run with sample data (dry run: commands are simulated, nothing is written).
//   - Versioning: save draft, publish new version, compare versions, roll back.
//   - Start from a template of a module or from a recording (WorkflowRecorder).
//   - Permissions: who may edit/publish (personal vs. tenant workflows).
