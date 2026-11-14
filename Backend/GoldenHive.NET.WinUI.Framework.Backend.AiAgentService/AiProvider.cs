// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.AiAgentService
// File    : AiProvider.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Abstraction over the AI model provider.
//
// BEHAVIOUR
//   - Model profiles configured centrally (Configuration Service): provider, model,
//     temperature, token limits, region / data residency.
//   - Tool calling and structured output as the only interaction style (no free-text parsing).
//   - Keys from the secret store; per-tenant usage metering for cost limits.
//   - Replaceable provider so tenants can choose an approved model.
