// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.UpdateOrchestrator
// File    : RolloutPlanner.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Plans rollouts.
//
// BEHAVIOUR
//   - Stages: pilot group -> subset of tenants -> everyone.
//   - Backend first, backward compatible; then release the client module.
//   - Per update decide: hot-swap possible or restart required (manifest hotSwap).
//   - Watch error rates and health checks; trigger a rollback on problems.
//   - Send a module.version.available event to NotificationPoint.
//
// WORKFLOW IMPACT
//   - Before advancing a rollout stage, read the workflow impact summary.
//   - Tenant policy decides: continue, hold the stage while Broken workflows exist,
//     or continue and keep affected tenants on the previous module version until their
//     workflows were reviewed.
