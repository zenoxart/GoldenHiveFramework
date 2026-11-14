// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.WorkflowContracts
// File    : WorkflowDefinition.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   A versioned, declarative description of a workflow. Data only - never code -
//   so it can be stored, versioned, analysed, migrated and executed by a human or by AI.
//
// FIELDS
//   - WorkflowId, Version (incremented on every published change), Name, Description.
//   - Origin: ModuleTemplate (shipped by a module), Tenant (shared in a tenant), User (personal).
//   - OwningModule: the module the workflow mainly runs in (a workflow may use steps of
//     several modules; each use is listed in Dependencies).
//   - Steps: ordered list of WorkflowSteps; StartStepId.
//   - Transitions: WorkflowTransitions between steps.
//   - Variables: declared workflow variables with types (filled by inputs and step outputs).
//   - Triggers: list of WorkflowTriggers (how a run can start).
//   - AiPolicy: AiExecutionPolicy (whether and how AI may run it).
//   - Dependencies: WorkflowDependencies, computed when the definition is saved.
//   - Status: Draft, Active, Disabled, NeedsReview (set by the impact analysis), Archived.
//   - Permissions: who may view, edit, start the workflow.
//
// RULES
//   - Running instances keep the definition version they started with.
//   - A published version is immutable; edits create a new draft version.
