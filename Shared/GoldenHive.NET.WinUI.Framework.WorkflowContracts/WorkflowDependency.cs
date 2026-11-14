// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.WorkflowContracts
// File    : WorkflowDependency.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   One thing a workflow relies on. Computed from the definition when it is saved;
//   the basis of the impact analysis after module updates.
//
// KINDS
//   - Module (module ID + version range at the time of saving)
//   - Command (command ID + parameter schema version)
//   - InputSchema (schema ID + version)
//   - StepType (module step type ID + version)
//   - Event (event type + payload schema version)
//   - BackendApi (service + api version)
//
// FIELDS
//   Kind, ID, version, and the steps that use it.
