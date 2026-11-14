// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Workflows
// File    : WorkflowImpactViewModel.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Overview of every workflow affected by module updates. A ViewModelAppObject.
//
// CONTENTS
//   - List of module updates (module, from -> to version, date, rollout stage) with counts
//     per severity: Broken, ReviewRequired, AutoMigrated, Compatible.
//   - Per update: affected workflows with severity, affected steps, the reason
//     ("input field 'amount' changed from text to number"), affected running instances
//     and suspended AI triggers.
//   - Actions per workflow: open in designer, apply suggested migration, re-enable
//     AI triggers after review, disable, pin to the previous module version (if the
//     Update Orchestrator still offers it).
//   - Filters: my workflows, tenant workflows, module, severity, AI-triggered only.
//   - Fed by the WorkflowService; refreshed by workflow.impact.* events.
