// =============================================================================
// Project : GoldenHive.NET.Modules.DocumentManagement.Backend
// File    : _Project.cs
// Layer   : Module DocumentManagement
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PROJECT
//   ASP.NET Core API (net10.0) as the Docker container "document-service", route /api/documents behind the API Gateway.
//
// RESPONSIBILITIES
//   - The module's domain logic and data (own database or schema).
//   - Server-side authorization of every operation (documents.view, documents.edit).
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
