// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Host.Client
// File    : ClientAppContext.cs
// Layer   : Client / Host
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The application context of the WinUI client. Derives from ApplicationContext and adds
//   all framework services. Equivalent of DatenbankAppKontext in WIFI.Anwendung.Datenbank.
//   Replaces a generic DI host: the context is the composition root and service locator.
//
// LAZY SERVICES (each produced on first access via Produce<T>() and cached)
//   - Windows        : WindowManager (Windowing)
//   - Commands       : RelayCommandPool - the central command manager (Commands)
//   - Modules        : ModuleLoader (ModuleLoader)
//   - XamlMetadata   : ModuleXamlMetadataProvider (XamlMetadata)
//   - HotSwap        : HotSwapManager (HotSwap)
//   - Navigation     : NavigationService (Navigation)
//   - Notifications  : NotificationHubClient + router (NotificationClient)
//   - Security       : AuthenticationService, PermissionService (Security)
//   - Api            : HttpClient factory configured for the API Gateway
//   - Settings       : user settings (JSON in RoamingDataPath)
//   inherited: Log, Languages, Cache, paths.
//
// DISPATCHER
//   - Dispatcher (inherited, IUiDispatcher) is set to Commands.Dispatcher right after the
//     command manager is produced - the command pool owns the UI dispatcher.
//
// PRODUCTION (override of Produce<T>)
//   1. base.Produce<T>() (context set, error handler attached, log entry written).
//   2. Additionally route ErrorOccurred of framework services to the InfoBar area of the shell
//      (via Notifications) in addition to the log.
//   3. Except for the LogManager itself, to avoid recursion.
//
// FACTORY FOR MODULES
//   - CreateModuleContext(manifest): produces a ModuleAppContext whose parent is this context.
//
// RULES
//   - Exactly one instance, created in Program.Main before the WinUI Application.
//   - Lives until the process ends; ShutdownAsync() saves settings and window positions,
//     disconnects notifications and unloads modules.
//
// WORKFLOWS
//   - Workflows : WorkflowManager (Workflows) - catalogue, runner, designer, impact overview,
//     AI hand-over and approval inbox.
//
// SHORTCUTS
//   - Shortcuts : Commands.Shortcuts (the ShortcutManager inside the command pool).
//
// DOCK LAYOUT
//   - Dock : DockManager (Docking) - document well, tool windows, floating windows, layouts.
//
// ICONS
//   - Icons : IconCatalog (XamlMetadata) - icon names -> WinUI icons, shell + module icons.
