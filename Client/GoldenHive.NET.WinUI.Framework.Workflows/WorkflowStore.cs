// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Workflows
// File    : WorkflowStore.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Persists workflow instances on the client.
//
// BEHAVIOUR
//   - Local copy of in-progress instances in LocalDataPath\Workflows (JsonController),
//     sensitive variables encrypted with DPAPI.
//   - Synchronizes with the WorkflowService; the server version wins on conflict,
//     local unsent steps are replayed if still valid.
//   - Offline: interactive steps that do not need the backend can continue; steps that
//     need the backend queue until the connection is back.
