// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Host.Client
// File    : AppXamlMetadataBridge.cs
// Layer   : Client / Host
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Bridges the App class and the XAML types of dynamically loaded modules.
//
// BACKGROUND
//   The WinUI XAML compiler generates a XamlMetaDataProvider for the app that only knows
//   assemblies referenced at build time. Otherwise the XAML framework cannot resolve types
//   from modules loaded later (Frame.Navigate, XamlReader, DataTemplates).
//
// TASK
//   - App (partial) additionally implements IXamlMetadataProvider.
//   - GetXamlType / GetXmlnsDefinitions first ask the app's generated provider, then the
//     ModuleXamlMetadataProvider (XamlMetadata project), which aggregates the providers
//     of all loaded modules.
//
// RISK
//   Verify the behaviour early with a prototype against the Windows App SDK version in use.
