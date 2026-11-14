// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.WorkflowContracts
// File    : WorkflowImpactReport.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Result of the impact analysis for one module update; shown in the client overview.
//
// FIELDS
//   - ModuleId, FromVersion, ToVersion, analysed at, rollout stage.
//   - SurfaceDiff: added / removed / changed commands, input schemas, step types and events
//     of the module (with compatible or breaking classification).
//   - Per affected workflow:
//       * Severity: Unaffected, Compatible (nothing to do), AutoMigrated (a migration was
//         applied), ReviewRequired (likely fine, a person should check), Broken (cannot run).
//       * Affected steps and the dependency that changed, with a human-readable reason.
//       * Running instances affected and what happened to them (continued, paused, migrated).
//       * Suggested action: migrate, open in designer, disable, pin to old module version.
//       * Whether AI triggers of this workflow were suspended.
