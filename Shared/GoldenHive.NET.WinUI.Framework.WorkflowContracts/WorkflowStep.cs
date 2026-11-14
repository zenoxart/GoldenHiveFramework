// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.WorkflowContracts
// File    : WorkflowStep.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   One step of a workflow.
//
// KINDS
//   - Input       : the user (or AI) fills a form described by an InputSchema.
//   - Choice      : pick one of several options; result drives the transition.
//   - Review      : show collected data for confirmation before continuing.
//   - Command     : run a command of the central command manager (command ID +
//                   parameter mapping from workflow variables).
//   - ModuleStep  : a step type registered by a module (IWorkflowStepHandler).
//   - WaitForEvent: pause until an event arrives (e.g. documents.approved), with timeout.
//   - Approval    : a human must approve (always human, never AI).
//   - AiTask      : free-form instruction for the AI agent with an explicit tool list
//                   and a required output schema.
//   - SubWorkflow : start another workflow and continue with its result.
//
// FIELDS
//   - StepId, Kind, Title/description resource keys.
//   - InputSchemaRef (Input steps): schema ID + version.
//   - CommandId / StepTypeId + parameter mapping (WorkflowExpressions over variables).
//   - OutputMapping: which outputs are written to which variables.
//   - Execution flags: RequiresHuman (UI-only step), AiAllowed, Critical (AI needs approval),
//     Compensation (step to run if a later step fails, for rollback).
//   - Timeout and retry policy.
