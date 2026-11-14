using GoldenHive.NET.WinUI.Framework.Host.Client.Views;
using GoldenHive.NET.WinUI.Framework.ModuleContracts;
using GoldenHive.NET.WinUI.Framework.Navigation;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;

/// <summary>Registers the built-in views of the shell and two demo modules.</summary>
internal static class BuiltInViews
{
    public const string Start = "shell.start";
    public const string Settings = "shell.settings";
    public const string ModuleExplorer = "shell.moduleexplorer";
    public const string Output = "shell.output";
    public const string DemoDocuments = "demo.documents";
    public const string DemoReports = "demo.reports";

    /// <summary>Views opened when no saved layout exists.</summary>
    public static readonly IReadOnlyList<string> DefaultLayout = [Start, ModuleExplorer, Output];

    public static void Register(ClientAppContext context)
    {
        var registry = context.Navigation.Registry;

        registry.Register(new NavigationTarget(Start, "shell")
        {
            TextKey = "shell:view.start",
            Icon = "home",
            Group = "general",
            Order = 0,
            IsModuleHome = true,
            ShowInViewMenu = true,
            View = new ViewDescriptor { Role = ViewRole.Document },
            CreateViewModel = c => c.Produce<StartViewModel>(),
            CreateView = vm => new StartView((StartViewModel)vm),
        });

        registry.Register(new NavigationTarget(Settings, "shell")
        {
            TextKey = "shell:view.settings",
            Icon = "settings",
            Group = "general",
            Order = 9,
            ShowInViewMenu = true,
            View = new ViewDescriptor { Role = ViewRole.Document, MinWidth = 480, MinHeight = 400 },
            CreateViewModel = c => c.Produce<SettingsViewModel>(),
            CreateView = vm => new SettingsView((SettingsViewModel)vm),
        });

        registry.Register(new NavigationTarget(ModuleExplorer, "shell")
        {
            TextKey = "shell:view.moduleexplorer",
            Icon = "library",
            Group = "general",
            Order = 1,
            ShowInViewMenu = true,
            View = new ViewDescriptor { Role = ViewRole.ToolWindow, DefaultDock = DockSide.Left, CanClose = true },
            CreateViewModel = c => c.Produce<ModuleExplorerViewModel>(),
            CreateView = vm => new ModuleExplorerView((ModuleExplorerViewModel)vm),
        });

        registry.Register(new NavigationTarget(Output, "shell")
        {
            TextKey = "shell:view.output",
            Icon = "output",
            Group = "general",
            Order = 2,
            ShowInViewMenu = true,
            View = new ViewDescriptor { Role = ViewRole.ToolWindow, DefaultDock = DockSide.Bottom },
            CreateViewModel = c => c.Produce<OutputViewModel>(),
            CreateView = vm => new OutputView((OutputViewModel)vm),
        });

        RegisterDemo(registry, DemoDocuments, "document", 1);
        RegisterDemo(registry, DemoReports, "list", 2);
    }

    private static void RegisterDemo(NavigationRegistry registry, string key, string icon, int order) =>
        registry.Register(new NavigationTarget(key, key)
        {
            TextKey = "shell:" + key,
            Icon = icon,
            Group = "demo",
            Order = order,
            IsModuleHome = true,
            View = new ViewDescriptor { Role = ViewRole.Document },
            CreateViewModel = c =>
            {
                var viewModel = c.Produce<DemoModuleViewModel>();
                viewModel.Initialize(key);
                return viewModel;
            },
            CreateView = vm => new DemoModuleView((DemoModuleViewModel)vm),
        });
}
