// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : _Project.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PROJECT
//   WinUI class library, TargetFramework net10.0-windows10.0.26100.0.
//   Self-written MVVM and application infrastructure - no third-party MVVM library.
//   Modelled on the AppKontext architecture of WIFI.Anwendung
//   (github.com/zenoxart/WIFI.CS.2021/tree/master/WIFI.Anwendung), translated to English
//   and adapted to WinUI 3.
//
// ROLE
//   Lowest client layer. Every other client library and every module builds on it:
//   all framework services are AppObjects that are produced by an ApplicationContext.
//
// CONTENTS
//   - IAppContext            "has an application context" (WIFI: IAppKontext)
//   - IUiDispatcher          UI dispatcher contract (implemented inside the RelayCommandPool)
//   - ApplicationContext     the infrastructure + object factory (WIFI: AppKontext)
//   - AppObject              base class of all application objects (WIFI: AppObjekt)
//   - ErrorOccurredEventArgs error event data (WIFI: FehlerAufgetreten)
//   - ViewModelAppObject     base class of all ViewModels (WIFI: ViewModelAppObjekt)
//   - IStatefulViewModel     state that survives a hot-swap
//   - LogManager             application log (WIFI: ProtokollManager)
//   - LanguageManager        application languages (WIFI: SprachenManager)
//   - JsonController<T>      typed load/save of data lists (WIFI: XmlController<T>)
//
// MAPPING WIFI -> ZENOX
//   AppKontext -> ApplicationContext   Produziere<T>() -> Produce<T>()
//   AppObjekt -> AppObject             FehlerAufgetreten -> ErrorOccurred
//   ViewModelAppObjekt -> ViewModelAppObject
//   DatenbankAppKontext -> ClientAppContext (host)   FensterManager -> WindowManager (Windowing)
//   ProtokollManager -> LogManager     SprachenManager -> LanguageManager
//   XmlController<T> -> JsonController<T>
//
// NAMING NOTE
//   The class is called ApplicationContext, not AppContext, because AppContext would clash
//   with System.AppContext wherever "using System;" is in scope (implicit usings).
//
// RESPONSIVE
//   SizeClass / IResponsiveView - responsive size classes for all views.
