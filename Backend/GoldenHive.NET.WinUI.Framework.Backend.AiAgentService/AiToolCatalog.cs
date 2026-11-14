// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.AiAgentService
// File    : AiToolCatalog.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Turns workflow building blocks into tools the AI model can call.
//
// BEHAVIOUR
//   - Only what the policy allows: the steps' commands and module step types marked
//     AiAllowed and headless-capable, plus read-only lookups the step needs (e.g. find customer).
//   - Tool schemas generated from InputSchemas and command parameter schemas.
//   - Never exposes Approval steps, UI-only steps or administrative commands.
