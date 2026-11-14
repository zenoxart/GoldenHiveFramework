// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.WorkflowService
// File    : WorkflowEngine.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Durable, headless workflow execution on the server.
//
// BEHAVIOUR
//   - Runs instances that need no UI: Command and ModuleStep steps via the module backends,
//     WaitForEvent, timeouts, schedules, sub-workflows, compensation.
//   - Hands interactive steps to the client (inbox task / push notification) and AI steps to
//     the AiAgentService.
//   - Each step executes on behalf of the instance's RunAs user (token exchange /
//     on-behalf-of), so server and AI runs have exactly the user's permissions.
//   - Exactly-once step execution: step results are stored with an idempotency key before
//     the next step starts; outbox for events (workflow.instance.*).
//
// MONAD ARCHITECTURE
//   - Every step execution returns Result<StepOutput>; the engine stores the Result per step with
//     the idempotency key. Failure kinds decide the reaction: Validation -> back to the human/AI with
//     field errors, Forbidden -> stop, Unavailable/Timeout -> retry with backoff, Unexpected ->
//     compensation and alert.
