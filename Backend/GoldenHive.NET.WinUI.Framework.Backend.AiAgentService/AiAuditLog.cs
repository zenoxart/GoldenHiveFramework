// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.AiAgentService
// File    : AiAuditLog.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Traceability of every AI action.
//
// CONTENTS
//   - Per AI step: model profile, prompt context hash, proposed values, policy decisions,
//     approvals (who, when), executed result, cost.
//   - Visible in the instance history in the client; exportable for compliance reviews.
