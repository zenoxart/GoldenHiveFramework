// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.XamlMetadata
// File    : ModuleXamlMetadataProvider.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Aggregates the IXamlMetadataProvider of every loaded module.
//
// BEHAVIOUR
//   - Register(moduleId, provider) / Unregister(moduleId).
//   - GetXamlType(Type | string): ask the providers in turn; the first match wins.
//   - After a module is unloaded, no references to its provider may remain.
//
// USED BY
//   AppXamlMetadataBridge in the host.
