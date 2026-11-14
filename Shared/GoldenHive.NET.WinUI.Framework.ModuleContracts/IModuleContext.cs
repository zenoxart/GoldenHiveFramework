// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleContracts
// File    : IModuleContext.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   What the host makes available to a module, as a UI-free contract.
//   Implemented by ModuleAppContext (ModuleLoader), which is also the module's
//   ApplicationContext - module code casts nothing and uses the context directly.
//
// CONTENTS
//   - Module ID, version, module data folder.
//   - Registration APIs: commands (with placements), navigation targets, event subscriptions,
//     command tree providers - all tracked and removed automatically on unload.
//   - Access to an HttpClient preconfigured for the module's backend route,
//     the module's settings and resources.
//
// WORKFLOW REGISTRATION
//   - RegisterWorkflowStepType(IWorkflowStepHandler) - custom step types of the module.
//   - RegisterWorkflowTemplate(WorkflowDefinition) - templates shipped with the module.
//   - Both are tracked and removed automatically when the module unloads.
//
// DOCK LAYOUT
//   - RegisterView(navigationKey, ViewDescriptor): role and default dock position of a view;
//     tool windows automatically appear under "View > Other Windows".
//
// ICONS AND TEXTS
//   - RegisterIcon(name, source): module icons, available as "<moduleId>.<name>".
//   - Module texts are registered automatically from the package (languages from the manifest).
