// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleContracts
// File    : IModuleEntryPoint.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Entry point of every client module (manifest field entryType).
//
// METHODS (described)
//   - Initialize(IModuleContext): register services, commands, navigation targets,
//     views and event subscriptions.
//   - GetXamlMetadataProvider(): return the module's generated provider (or none).
//   - ShutdownAsync(): release resources, close connections, detach handlers.
