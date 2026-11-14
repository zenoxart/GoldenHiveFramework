// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.WorkflowContracts
// File    : WorkflowInstance.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   A running or finished execution of a workflow definition version.
//
// FIELDS
//   - InstanceId, WorkflowId + Version, TriggeredBy (trigger + payload reference), RunAs user.
//   - Executor: Human, Ai, or Mixed (handed over between them).
//   - State: Running, WaitingForInput, WaitingForApproval, WaitingForEvent,
//     Paused (e.g. module update), Completed, Failed, Cancelled, Compensated.
//   - CurrentStepId, Variables (sensitive values encrypted at rest).
//   - History: one entry per executed step - actor (user or AI), inputs, outputs,
//     duration, result, AI reasoning summary; append-only (audit trail).
//
// RULE
//   The instance is persisted after every step, so it can resume after a crash, on another
//   device, on the server, or by another executor.
