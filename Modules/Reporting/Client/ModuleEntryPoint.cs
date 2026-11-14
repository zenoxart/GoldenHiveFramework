// =============================================================================
// Project : GoldenHive.NET.Modules.Reporting.Client
// File    : ModuleEntryPoint.cs
// Layer   : Module Reporting
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Entry point of the reporting module (IModuleEntryPoint).
//
// INITIALIZE
//   - Receives its ModuleAppContext; produces its services and ViewModels only through
//     context.Produce<T>() (API client for /api/reports).
//   - Register commands prefixed with the module ID with the central command manager,
//     each with its placements (main menu, context menus, navigation tree).
//   - Register group descriptors for the module's own submenus if needed.
//   - Add navigation targets and menu items (report catalogue, parameter dialog, result view).
//   - Subscribe to events with the NotificationRouter.
//
// SHUTDOWN
//   Cancel running requests, detach subscriptions and handlers, keep no static references.
//
// WORKFLOWS
//   - Register the module's workflow step types (IWorkflowStepHandler) and input schemas.
//   - Register workflow templates, e.g.: Monthly report: schedule (1st of month) -> choose period -> run report -> review -> distribute.
//   - Mark commands as WorkflowCapable / AiAllowed in their descriptors.
//
// NAVIGATION WHEEL
//   - The module's home target becomes its segment in the wheel automatically.
//   - Register the most important views with a placement under shell.wheel/<moduleId>
//     so they appear in the module's outer ring.
//
// SHORTCUTS
//   - Suggest DefaultShortcuts in the command descriptors, scoped to the module or its views
//     (never Global unless really needed); reserved shell shortcuts are rejected.
//
// DOCK LAYOUT
//   - Register a ViewDescriptor per view: main views as Documents, helper panels (e.g. filters,
//     details, previews) as ToolWindows with a default dock side.
