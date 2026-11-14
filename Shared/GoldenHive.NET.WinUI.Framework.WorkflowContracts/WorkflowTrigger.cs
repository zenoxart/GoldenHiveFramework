// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.WorkflowContracts
// File    : WorkflowTrigger.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Defines how a workflow run starts and who executes it.
//
// KINDS
//   - Manual   : started by a user (workflow list, command palette, menu).
//   - Command  : after a given command ran (e.g. documents.create).
//   - Event    : a NotificationPoint event matches (type + filter expression on the payload),
//                e.g. documents.created where category = "invoice".
//   - Schedule : cron expression in the tenant's time zone.
//   - Condition: a periodically evaluated query of a module API turns true.
//
// FIELDS
//   - Kind + configuration, payload-to-variable mapping.
//   - Executor: Human (create a task in the user's inbox) or Ai (start the AI agent).
//   - RunAs: the user on whose behalf the run executes (the AI never gets more rights).
//   - Enabled flag, de-duplication key, maximum concurrent runs.
