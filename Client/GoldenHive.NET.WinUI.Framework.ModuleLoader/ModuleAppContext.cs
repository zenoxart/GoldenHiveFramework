// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleLoader
// File    : ModuleAppContext.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Child application context per loaded module version. Derives from ApplicationContext
//   and implements IModuleContext. Created by ClientAppContext.CreateModuleContext(manifest).
//
// BEHAVIOUR
//   - Parent: the ClientAppContext. Infrastructure services (Log, Languages, Dispatcher,
//     Commands, Navigation, Notifications, Security, Windows) are passed through from the
//     parent - a module never gets its own copies of framework services.
//   - Produce<T>(): produces module objects (ViewModels, module services) with this context,
//     so they automatically know their module; log entries are prefixed with the module ID.
//   - Tracks everything the module registers through it (commands, command placements,
//     navigation targets, event subscriptions, tree providers) and every produced object
//     that is IDisposable.
//   - Module-specific paths: ModuleDataPath = LocalDataPath\Modules\<moduleId>.
//   - Api: HttpClient preconfigured for the module's backend route.
//
// UNLOAD / HOT-SWAP
//   - Release(): unregister everything tracked, dispose produced objects, detach all
//     ErrorOccurred handlers, drop the reference to the parent.
//   - After Release no reference from the host may lead into the module's load context.
//
// WORKFLOWS
//   Also tracks workflow step types and templates the module registers; on Release they are
//   removed from the WorkflowManager (running instances were paused before by the HotSwapManager).
