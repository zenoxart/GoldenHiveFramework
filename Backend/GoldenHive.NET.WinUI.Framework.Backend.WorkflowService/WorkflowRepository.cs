// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.WorkflowService
// File    : WorkflowRepository.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Storage of workflow definitions, versions and instances.
//
// BEHAVIOUR
//   - Definitions per tenant/user with full version history; published versions immutable.
//   - Instances with append-only step history (audit trail); sensitive variables encrypted.
//   - Index of WorkflowDependencies -> fast lookup "which workflows use command X / schema Y".
//   - Retention rules for finished instances per tenant.
