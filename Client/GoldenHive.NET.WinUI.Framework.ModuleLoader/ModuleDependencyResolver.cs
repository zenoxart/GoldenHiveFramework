// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleLoader
// File    : ModuleDependencyResolver.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Checks dependencies between modules and determines the load order.
//
// BEHAVIOUR
//   - Check version ranges (e.g. >=1.2.0 <3.0.0) against loaded/available versions.
//   - Topological sort; report cycles and missing modules as errors.
//   - During a hot-swap: determine dependent modules that must be swapped too.
