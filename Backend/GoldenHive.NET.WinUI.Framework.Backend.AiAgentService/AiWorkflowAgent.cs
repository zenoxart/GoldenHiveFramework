// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.AiAgentService
// File    : AiWorkflowAgent.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Drives a workflow instance with an AI model.
//
// SEQUENCE PER STEP
//   1. Build the context: workflow goal, current step, its InputSchema, allowed tools,
//      variables within the policy's data scope, trigger payload, short history.
//   2. Ask the model for the step's inputs or the next action (structured output that must
//      validate against the InputSchema / tool schema).
//   3. Check against the AiExecutionPolicy (AiGuardrails). Critical or policy-required
//      steps -> create an approval request and park the instance.
//   4. Execute through the WorkflowEngine (same handlers and permissions as a human run).
//   5. Record inputs, outputs and a reasoning summary in the instance history (AiAuditLog).
//   6. On low confidence, validation failure after retries, or a policy limit: hand the
//      instance over to a human (inbox task) instead of guessing.
