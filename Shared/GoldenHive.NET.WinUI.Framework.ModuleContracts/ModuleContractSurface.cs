// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleContracts
// File    : ModuleContractSurface.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Everything a module version offers to workflows and to other parts of the system.
//   Published with every module version; the Module Registry diffs it between versions,
//   and the WorkflowService uses the diff for its impact analysis.
//
// CONTENTS
//   - Commands: command IDs with parameter schema versions, WorkflowCapable / AiAllowed flags.
//   - Input schemas: schema IDs and versions.
//   - Workflow step types: IDs, versions, input/output schemas, headless capability.
//   - Events: event types the module publishes and their payload schema versions.
//   - Backend API version.
//   - Workflow templates shipped with the module.
//   - Migrations: declared renames/replacements to the previous version
//     (used by WorkflowMigration).
//
// GENERATION
//   Generated in CI from the module's registrations (not written by hand) and signed together
//   with the manifest, so it always matches the shipped code.
