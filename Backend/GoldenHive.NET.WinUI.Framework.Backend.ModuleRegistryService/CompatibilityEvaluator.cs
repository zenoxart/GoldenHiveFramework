// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.ModuleRegistryService
// File    : CompatibilityEvaluator.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Server-side compatibility check.
//
// BEHAVIOUR
//   Same rules as ModuleCompatibility (Shared): host version, Windows App SDK range,
//   contract version, dependencies. Returns a reason when a module is filtered out.
