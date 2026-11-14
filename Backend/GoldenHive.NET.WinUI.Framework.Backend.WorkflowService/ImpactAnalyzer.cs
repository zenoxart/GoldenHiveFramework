// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.WorkflowService
// File    : ImpactAnalyzer.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Determines which workflows are affected by a module update.
//
// SEQUENCE
//   1. Triggered by the Module Registry when a new module version is published (or reaches
//      a rollout stage), with the contract surface diff of old vs. new version.
//   2. Look up all workflow definitions with a dependency on that module (dependency index).
//   3. Classify every affected workflow:
//        Compatible      - only additions / compatible changes,
//        AutoMigrated    - a registered migration (WorkflowMigration) applies cleanly,
//        ReviewRequired  - semantic changes (e.g. new required field with a default),
//        Broken          - removed command/step type, incompatible schema change.
//   4. Apply automatic migrations as new draft or published versions (per tenant setting).
//   5. Mark Broken/ReviewRequired definitions as NeedsReview and suspend their AI triggers.
//   6. Decide for running instances: continue on old version, migrate at the next step
//      boundary, or pause.
//   7. Store the WorkflowImpactReport and publish workflow.impact.detected via the event bus,
//      so clients show the overview and owners get notified.
//   8. Report the totals to the Update Orchestrator (it may hold a rollout stage when
//      critical workflows would break).
