// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Workflows
// File    : WorkflowStepPresenter.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Shows a workflow step to the user.
//
// BEHAVIOUR
//   - Generates the form from the InputSchema (fields, labels, validation messages) with
//     standard WinUI controls, or uses the module's custom view if the step type provides one.
//   - Header with workflow name and step progress (step x of y, or a breadcrumb of
//     completed steps); Back, Next, Cancel, "Let AI continue".
//   - Shows who filled a value (user or AI) and the AI's short reasoning on hover.
//   - Shown as a page in the content frame or in a separate window (WindowManager).
//
// RESPONSIVE AND DETACHABLE
//   - Generated forms lay out fields in 1 column (Compact), 2 (Medium/Expanded) or 3 (Wide);
//     buttons wrap; long workflows show a compact step indicator in Compact.
//   - A running workflow can be detached into its own window like any view.
