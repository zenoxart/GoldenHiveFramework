using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.Docking;
using GoldenHive.NET.WinUI.Framework.HotSwap;
using GoldenHive.NET.WinUI.Framework.ModuleLoader;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using GoldenHive.NET.WinUI.Framework.Navigation;
using GoldenHive.NET.WinUI.Framework.NotificationClient;
using GoldenHive.NET.WinUI.Framework.Security;
using GoldenHive.NET.WinUI.Framework.Windowing;
using GoldenHive.NET.WinUI.Framework.Workflows;
using GoldenHive.NET.WinUI.Framework.XamlMetadata;
using ModuleLoaderService = GoldenHive.NET.WinUI.Framework.ModuleLoader.ModuleLoader;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;

/// <summary>
/// The application context of the WinUI client (WIFI: DatenbankAppKontext). Adds every framework service
/// as a lazily produced property; it is the composition root of the host and the IModuleHost the modules see
/// (each module gets a ModuleAppContext that forwards to it).
/// </summary>
public sealed class ClientAppContext : ApplicationContext, IModuleHost, IWindowContext, IWorkflowContext
{
    private RelayCommandPool? _commands;
    private IconCatalog? _icons;
    private WindowManager? _windows;
    private DockManager? _dock;
    private NavigationService? _navigation;
    private HostSettingsStore? _settings;
    private BackendConnection? _backend;
    private UserProfileService? _profile;
    private NotificationHubClient? _hub;
    private NotificationRouter? _router;
    private InAppNotificationService? _inApp;
    private AppNotificationService? _toasts;
    private PushNotificationService? _push;
    private ModuleLoaderService? _modules;
    private WorkflowManager? _workflows;
    private HotSwapManager? _hotSwap;

    public override string Company => "ZenoxArt";

    public override string Title => "GoldenHive";

    /// <summary>The central command manager (incl. dispatcher and shortcut manager).</summary>
    public RelayCommandPool Commands => GetOrProduce(ref _commands);

    public IconCatalog Icons => GetOrProduce(ref _icons);

    public WindowManager Windows => GetOrProduce(ref _windows);

    public DockManager Dock => GetOrProduce(ref _dock);

    public NavigationService Navigation => GetOrProduce(ref _navigation);

    public HostSettingsStore Settings => GetOrProduce(ref _settings);

    /// <summary>Connection to the Golden Hive gateway (sign-in, tokens, permissions, HTTP).</summary>
    public BackendConnection Backend => GetOrProduce(ref _backend);

    /// <summary>Profile of the signed-in user (picture, password).</summary>
    public UserProfileService Profile => GetOrProduce(ref _profile);

    /// <summary>Server-sent event stream of the NotificationPoint.</summary>
    public NotificationHubClient Hub => GetOrProduce(ref _hub);

    public NotificationRouter Router => GetOrProduce(ref _router);

    /// <summary>In-app notification centre (also receives every routed event).</summary>
    public InAppNotificationService InApp => GetOrProduce(ref _inApp);

    /// <summary>Windows toasts while the app is in the background.</summary>
    public AppNotificationService Toasts => GetOrProduce(ref _toasts);

    /// <summary>Optional WNS push channel (only used when the backend has WNS configured).</summary>
    public PushNotificationService Push => GetOrProduce(ref _push);

    public ModuleLoaderService Modules => GetOrProduce(ref _modules);

    public WorkflowManager Workflows => GetOrProduce(ref _workflows);

    public HotSwapManager HotSwap => GetOrProduce(ref _hotSwap);

    /// <summary>Modules register their workflow step types and templates with the workflow manager.</summary>
    public IWorkflowRegistrar? WorkflowRegistrar => Workflows;
}
