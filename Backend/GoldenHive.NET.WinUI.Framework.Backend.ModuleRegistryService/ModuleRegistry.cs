// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.ModuleRegistryService
// File    : ModuleRegistry.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Central directory of all modules and versions.
//
// BEHAVIOUR
//   - Publish manifests (from CI); status: draft, pilot, released, withdrawn.
//   - Enablement per tenant, role or user (e.g. pilot groups).
//   - Client query: return only enabled AND compatible versions.
//
// WORKFLOW IMPACT
//   - On publishing a module version (and on every rollout stage change), diff its
//     ModuleContractSurface against the previous version (added / removed / changed,
//     compatible or breaking).
//   - Send the diff to the WorkflowService (ImpactAnalyzer) and store the returned summary
//     with the module version, so the client overview and the Update Orchestrator can use it.
