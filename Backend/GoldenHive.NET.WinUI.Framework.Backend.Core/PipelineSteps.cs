// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.Core
// File    : PipelineSteps.cs
// Layer   : Backend / Core
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Reusable railway steps that every use case composes.
//
// STEPS
//   - ValidateStep<TRequest>: runs the request's Validation rules (all errors at once).
//   - AuthorizeStep: checks a permission for the current or RunAs user -> Forbidden.
//   - TransactionStep: runs the inner pipeline in a database transaction; commits only on
//     Success, rolls back on Failure (no exceptions needed for rollback).
//   - OutboxStep: writes domain events to the outbox inside the transaction.
//   - IdempotencyStep: returns the stored Result for a repeated idempotency key (workflow engine,
//     retries).
//   - AuditStep: records who did what (user, workflow engine, AI agent) with the outcome.
