// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Workflows
// File    : WorkflowManager.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Central workflow service of the client. An AppObject produced by the ClientAppContext
//   (Context.Workflows).
//
// RESPONSIBILITIES
//   - Catalogue: workflow definitions available to the user - module templates, tenant and
//     personal workflows - loaded from the WorkflowService and filtered by permission.
//   - Step types: registry of IWorkflowStepHandlers registered by modules through their
//     ModuleAppContext; removed automatically when a module unloads.
//   - Start, resume, cancel instances; hand an instance over to the AI ("let AI continue")
//     or take it back.
//   - Commands with placements: "workflows.start.<id>" in menus and the command palette,
//     "workflows.inbox", "workflows.designer", "workflows.impact", "workflows.approvals"
//     in the navigation tree - menus come from the central command manager as usual.
//   - Command triggers on the client: listens to executed commands and starts workflows
//     whose trigger matches (server-side triggers run in the WorkflowService).
//   - Module updates: subscribes to the HotSwapManager; before a swap, running instances of
//     that module are saved at a step boundary and paused; after the swap they continue,
//     are migrated or stay paused depending on the impact report.
//
// SHORTCUTS
//   Users can bind a shortcut to "workflows.start.<id>" in the shortcut settings to start a
//   workflow with a key combination.
//
// DOCK LAYOUT
//   Workflow designer and running workflows open as documents; inbox, AI approvals and the impact
//   overview are tool windows (bottom/right) that can be docked, auto-hidden or floated.
