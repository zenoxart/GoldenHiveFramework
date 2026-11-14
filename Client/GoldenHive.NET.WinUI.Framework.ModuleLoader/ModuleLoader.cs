// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleLoader
// File    : ModuleLoader.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Orchestrates loading and unloading of a client module.
//
// LOAD SEQUENCE
//   1. Check the manifest: host version, Windows App SDK range, dependencies.
//   2. Look up the package locally via ModuleStore, otherwise fetch it via ModulePackageDownloader.
//   3. ModulePackageValidator: verify hash and signature. On failure: abort.
//   4. Create a new ModuleLoadContext for moduleId + version, load the entry assembly.
//   5. Find the entry type via reflection; it must implement IModuleEntryPoint.
//   6. Register the module's XAML metadata with the ModuleXamlMetadataProvider.
//   7. Context.CreateModuleContext(manifest) -> ModuleAppContext for this module version.
//   8. EntryPoint.Initialize(moduleContext): the module registers services, commands,
//      navigation targets, views and notification subscriptions.
//   9. Report the "loaded" status to the shell and to NotificationPoint.
//
// UNLOAD SEQUENCE
//   Reverse order: EntryPoint.ShutdownAsync, ModuleAppContext.Release(); then hand the
//   AssemblyLoadContext over to the UnloadMonitor.
//
// ORDER
//   Load dependencies first (ModuleDependencyResolver provides the order).
