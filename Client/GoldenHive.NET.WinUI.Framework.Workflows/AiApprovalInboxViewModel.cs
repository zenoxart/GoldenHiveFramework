// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Workflows
// File    : AiApprovalInboxViewModel.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Inbox for steps the AI agent wants a human to approve. A ViewModelAppObject.
//
// CONTENTS
//   - Per pending approval: workflow, step, trigger, proposed inputs/action, the AI's
//     reasoning summary, data it used, and the policy rule that requires approval.
//   - Actions: approve, edit and approve, reject (with comment, fed back to the agent),
//     take over the instance manually.
//   - Notifications through NotificationPoint (toast when the app is in the background).
