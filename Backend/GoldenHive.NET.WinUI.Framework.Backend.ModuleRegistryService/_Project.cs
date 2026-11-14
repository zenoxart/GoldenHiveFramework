// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.ModuleRegistryService
// File    : _Project.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PROJECT
//   ASP.NET Core service (net10.0), run as its own Docker container.
//   Management of modules, versions, dependencies and enablement.
//
// CODE STYLE: MONAD ARCHITECTURE
//   All operations are use cases built as Result pipelines (Backend.Core): validate, authorize,
//   load, decide, persist, publish, project. No exceptions for expected failures; endpoints only
//   map Results to HTTP via the ResultHttpMapper.
