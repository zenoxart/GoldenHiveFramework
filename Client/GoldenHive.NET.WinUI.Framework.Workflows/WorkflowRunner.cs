// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Workflows
// File    : WorkflowRunner.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Executes one workflow instance on the client as a state machine.
//
// SEQUENCE PER STEP
//   1. Resolve the step and evaluate its input mapping.
//   2. Interactive steps: show the step through the WorkflowStepPresenter and wait for input;
//      AI-assisted mode: show the AI's proposed values pre-filled for the user to confirm.
//   3. Validate (client-side rules, then the module's ValidateAsync / server validation).
//   4. Execute: Command steps through the central command manager, ModuleStep through the
//      module's handler, Approval/WaitForEvent by parking the instance.
//   5. Write outputs to variables, append a history entry, persist the instance.
//   6. Evaluate transitions; continue, finish, or run compensation steps on failure.
//
// BEHAVIOUR
//   - Back/next within the workflow; going back keeps entered values.
//   - Persist after every step (WorkflowStore), so the user can close the app and resume
//     later - on this or another device.
//   - Long-running and waiting instances are moved to the server engine.
//   - All state changes go through the command pool's dispatcher to the UI.
