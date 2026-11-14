// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.WorkflowContracts
// File    : _Project.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PROJECT
//   Contracts for workflows, shared by the client, the backend services and modules.
//   Class library net10.0, no UI dependencies.
//
// THREE CAPABILITIES
//   1. Define workflows over several input interactions inside a module (in the app).
//   2. Show which workflows are affected when a module is updated (impact analysis).
//   3. Let AI run a workflow based on triggers, under explicit policies.
//
// CONTENTS
//   WorkflowDefinition, WorkflowStep, InputSchema, WorkflowTransition, WorkflowTrigger,
//   WorkflowInstance, WorkflowDependency, WorkflowImpactReport, AiExecutionPolicy,
//   IWorkflowStepHandler, WorkflowExpression.
