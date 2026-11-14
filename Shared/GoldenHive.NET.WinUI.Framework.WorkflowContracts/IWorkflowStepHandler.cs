// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.WorkflowContracts
// File    : IWorkflowStepHandler.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Contract a module implements to offer its own workflow step types.
//
// METHODS (described)
//   - Describe(): step type ID + version, input schema, output schema, display info,
//     whether it can run headless (needed for AI and server-side execution).
//   - ValidateAsync(inputs): check inputs before execution.
//   - ExecuteAsync(inputs, context, CancellationToken): perform the step without UI,
//     normally by calling the module's backend API; returns outputs.
//   - CreateInteractiveView(...) (client only, optional): custom UI for the step;
//     otherwise the form is generated from the input schema.
//
// RULE
//   Headless execution must call the module backend, never the module's UI, so the same
//   step works for a user in the app, for the server-side engine and for the AI agent.
//
// MONAD ARCHITECTURE
//   ValidateAsync returns Validation<Inputs>; ExecuteAsync returns Task<Result<Outputs>> - step
//   handlers call the module backend's use cases and never throw for expected failures.
