// =============================================================================
// Project : GoldenHive.NET.Modules.Reporting.Backend
// File    : _Project.cs
// Layer   : Module Reporting
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PROJECT
//   ASP.NET Core API (net10.0) as the Docker container "reporting-module", route /api/reports behind the API Gateway.
//
// RESPONSIBILITIES
//   - The module's domain logic and data (own database or schema).
//   - Server-side authorization of every operation (reports.view, reports.run).
//   - Domain events via an outbox to the event bus -> NotificationPoint.
//   - Keep the API backward compatible; breaking changes only with a new apiVersion.
//
// WORKFLOW STEPS
//   - Headless endpoints for every workflow step type and workflow-capable command, used by
//     the client, the WorkflowEngine and the AI agent alike (always authorized for the
//     RunAs user).
//   - Server-side validation endpoints for the module's input schemas.
//
// CODE STYLE: MONAD ARCHITECTURE
//   Use cases as Result pipelines on top of Backend.Core and Monads; domain code is pure and
//   immutable; repositories return Option/Result; endpoints only map Results to HTTP.
