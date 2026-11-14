// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.WorkflowService
// File    : TriggerEngine.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Evaluates workflow triggers on the server.
//
// BEHAVIOUR
//   - Event triggers: consumes the event bus (same events as NotificationPoint), matches
//     type + filter, maps the payload to variables.
//   - Schedule triggers: cron per tenant time zone; missed runs after downtime are handled
//     by policy (skip or catch up).
//   - Condition triggers: polls module APIs at a configured interval.
//   - De-duplication and max concurrent runs per trigger.
//   - Starts the instance with the trigger's executor: Human -> inbox task for the user,
//     Ai -> AiAgentService. Suspended triggers (impact analysis) are skipped and logged.
