using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.Docking;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using GoldenHive.NET.WinUI.Framework.Navigation;
using GoldenHive.NET.WinUI.Framework.Windowing;
using GoldenHive.NET.WinUI.Framework.XamlMetadata;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;

/// <summary>
/// The application context of the WinUI client (WIFI: DatenbankAppKontext). Adds every framework service
/// as a lazily produced property; it is the composition root of the host.
/// </summary>
public sealed class ClientAppContext : ApplicationContext, ICommandContext, IIconContext, IWindowContext, IDockContext, INavigationContext
{
    private RelayCommandPool? _commands;
    private IconCatalog? _icons;
    private WindowManager? _windows;
    private DockManager? _dock;
    private NavigationService? _navigation;
    private HostSettingsStore? _settings;

    public override string Company => "ZenoxArt";

    public override string Title => "GoldenHive";

    /// <summary>The central command manager (incl. dispatcher and shortcut manager).</summary>
    public RelayCommandPool Commands => GetOrProduce(ref _commands);

    public IconCatalog Icons => GetOrProduce(ref _icons);

    public WindowManager Windows => GetOrProduce(ref _windows);

    public DockManager Dock => GetOrProduce(ref _dock);

    public NavigationService Navigation => GetOrProduce(ref _navigation);

    public HostSettingsStore Settings => GetOrProduce(ref _settings);
}
