// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.IdentityService
// File    : PermissionCatalog.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Management of permissions.
//
// BEHAVIOUR
//   - Permissions come from the module manifests (permissions).
//   - Mapped to roles per tenant; roles mapped to users.
//   - On change, send a permissions.changed event to NotificationPoint.
//
// ON-BEHALF-OF TOKENS
//   - Token exchange for the WorkflowService and AiAgentService: they act for the RunAs
//     user of a workflow instance with that user's permissions only, marked as delegated
//     (actor = workflow engine or AI agent) for auditing.
