// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : ApplicationContext.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Provides the infrastructure of the application and produces all application objects.
//   Equivalent of AppKontext in WIFI.Anwendung. Exactly one root instance exists per process;
//   derived contexts add services (ClientAppContext in the host, ModuleAppContext per module).
//
// OBJECT FACTORY
//   Produce<T>() where T : IAppContext, new()
//     1. Create the object.
//     2. Set its AppContext property to this context.
//     3. If it is an AppObject: attach a handler to ErrorOccurred that writes the error to the Log
//        (so no error of an application object goes unnoticed).
//     4. Write a "produced" entry to the log (type NewInstance).
//     5. Hook for derived contexts (virtual): further production steps, e.g. the
//        ModuleAppContext remembers produced objects so it can release them on unload.
//     6. Return the object.
//   The method is virtual so derived contexts extend production (as DatenbankAppKontext did).
//
// LAZY SERVICES (base context)
//   Each service is created on first access via Produce<T>() and cached in a private field:
//   - Log        : LogManager
//   - Languages  : LanguageManager
//   - Cache      : thread-safe key/value cache for the session
//   Derived contexts add their own services the same way.
//
// UI THREAD
//   - Dispatcher (IUiDispatcher): owned by the central command manager. The context only
//     holds a reference; the ClientAppContext sets it to Commands.Dispatcher at startup
//     (WIFI set Protokoll.Dispatcher = WpfApp.Dispatcher). Used by ViewModelAppObject and
//     LogManager to marshal changes onto the UI thread.
//
// PATHS (cached, directories are created if missing)
//   - LocalDataPath   : packaged -> ApplicationData.LocalFolder
//                       unpackaged -> %LocalAppData%\<Company>\<Title>\<Version>
//   - RoamingDataPath : packaged -> ApplicationData.RoamingFolder (or LocalFolder\Roaming)
//                       unpackaged -> %AppData%\<Company>\<Title>\<Version>
//   - ApplicationPath : folder the app was started from (AppContext.BaseDirectory,
//                       not Assembly.Location - empty for single-file builds)
//   - Company, title and version come from the entry assembly attributes; characters that are
//     invalid in paths are replaced (WIFI fix of 2021-01-28: "/" and "&").
//
// RULES
//   - The context contains infrastructure only - no business logic.
//   - All services are AppObjects; nothing outside the context news them up.
//   - Thread-safe lazy creation (services may be requested from background threads).
