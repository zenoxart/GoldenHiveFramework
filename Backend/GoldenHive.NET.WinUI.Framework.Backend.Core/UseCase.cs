// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.Core
// File    : UseCase.cs
// Layer   : Backend / Core
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The shape of every backend operation in the monad architecture.
//
// STRUCTURE OF A USE CASE (railway, each arrow = Bind / BindAsync)
//   request
//     -> Validate          : Validation of the input -> value objects        (Validation -> Result)
//     -> Authorize         : permission of the (RunAs) user for this action  (Forbidden)
//     -> Load              : repository lookup                               (Option -> NotFound)
//     -> Decide            : pure domain logic on immutable data, no I/O     (Result)
//     -> Persist           : within a transaction                            (Try at the adapter)
//     -> Publish           : domain events into the outbox, same transaction
//     -> Project           : map to the response DTO                         (Map)
//   -> Result<Response>
//
// RULES
//   - One use case per class/file, named after the action (ApproveInvoice, CreateUser).
//   - Input and output are immutable records; the domain is pure (no DbContext, no HttpClient,
//     no DateTime.Now - time and IDs come from injected providers).
//   - Dependencies via ordinary ASP.NET Core dependency injection (constructor); the monads are
//     used for control flow and errors, not as a dependency-injection mechanism.
//   - Workflow steps (headless handlers), AI tool calls and HTTP endpoints all call the same use
//     case, so humans, the workflow engine and the AI agent get identical behaviour and errors.
