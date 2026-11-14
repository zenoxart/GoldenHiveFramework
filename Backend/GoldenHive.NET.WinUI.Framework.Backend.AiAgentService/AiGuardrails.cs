// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.AiAgentService
// File    : AiGuardrails.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Enforces the AiExecutionPolicy before anything the AI proposes is executed.
//
// CHECKS
//   - Step and tool on the allow list; autonomy level requires approval?
//   - RunAs user has the permission (the AI never has more rights than that user).
//   - Limits: runs per day, steps per run, tokens/cost, run time.
//   - Data scope: sensitive fields removed from the prompt; output scanned before use.
//   - Values within schema and plausibility rules; repeated identical actions (loops) stopped.
//   - Kill switch per tenant, per workflow and globally.
//
// MONAD ARCHITECTURE
//   Each guardrail is a pipeline step returning Result: a violated rule yields Forbidden (or
//   Validation for out-of-schema values) with a code that is recorded in the audit log.
