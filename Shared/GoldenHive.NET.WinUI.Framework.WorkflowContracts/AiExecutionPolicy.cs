// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.WorkflowContracts
// File    : AiExecutionPolicy.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Controls whether and how the AI agent may execute a workflow.
//
// FIELDS
//   - Autonomy:
//       Off                  AI never runs this workflow.
//       Suggest              AI pre-fills inputs; a human executes every step.
//       ApproveCritical      AI runs, but steps flagged Critical wait for human approval.
//       ApproveEachStep      AI proposes each step; a human approves it.
//       Autonomous           AI runs all AiAllowed steps; Approval steps still need a human.
//   - Allowed steps / commands (allow list); everything else is out of reach for the AI.
//   - Approvers (users or roles) and approval timeout (then: cancel or escalate).
//   - Limits: max runs per day, max steps per run, max tokens / cost per run, time limit.
//   - Data scope: which variables and module data may be sent to the AI model;
//     Sensitive input fields are never sent.
//   - Model profile (from the Configuration Service), required confidence for automatic
//     choices, and the fallback when the AI is unsure (hand over to a human).
