// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Workflows
// File    : WorkflowInboxViewModel.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The user's workflow inbox. A ViewModelAppObject.
//
// CONTENTS
//   - Instances waiting for this user's input (started by a trigger with a human executor,
//     handed back by the AI, or assigned by another user).
//   - Running and recently finished instances, including AI runs, with their history.
//   - Resume, cancel, hand over to AI.
