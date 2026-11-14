using GoldenHive.NET.WinUI.Framework.CommandContracts;
using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.ModuleContracts;
using VirtualKey = Windows.System.VirtualKey;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;

/// <summary>Registers the commands of the shell with the central command manager.</summary>
internal static class ShellCommands
{
    private const string Module = ShortcutManager.ShellModuleId;
    private const string Menu = "shell.mainmenu";

    public static void Register(ClientAppContext context, Func<MainWindow?> mainWindow)
    {
        var pool = context.Commands;
        var dock = context.Dock;

        pool.RegisterGroup(Module, new CommandGroupDescriptor(Menu, "file") { TextKey = "shell:menu.file", Order = 0 });
        pool.RegisterGroup(Module, new CommandGroupDescriptor(Menu, "view") { TextKey = "shell:menu.view", Order = 1 });
        pool.RegisterGroup(Module, new CommandGroupDescriptor(Menu, "window") { TextKey = "shell:menu.window", Order = 2 });
        pool.RegisterGroup(Module, new CommandGroupDescriptor(Menu, "help") { TextKey = "shell:menu.help", Order = 3 });
        pool.RegisterGroup(Module, new CommandGroupDescriptor("shell.navigation", "general") { TextKey = "shell:group.general", Order = 0 });
        pool.RegisterGroup(Module, new CommandGroupDescriptor("shell.navigation", "demo") { TextKey = "shell:group.demo", Order = 1 });

        Add(pool, "shell.exit", "shell:command.exit", "power", [new(Menu, "file", 9)], [],
            _ => mainWindow()?.Close());

        Add(pool, "shell.palette", "shell:command.palette", "search", [new(Menu, "view", 2)], ["Ctrl+Shift+P"],
            _ => mainWindow()?.FocusPalette());

        Add(pool, "shell.navigationwheel.show", "shell:command.wheel", "switch", [new(Menu, "window", 0)], ["Ctrl+Tab", "Ctrl+Shift+Tab"],
            _ => mainWindow()?.Shell?.BeginWheel(ShortcutInputRouter.IsDown(VirtualKey.Shift)));

        Add(pool, "shell.view.detach", "shell:command.detach", "detach", [new(Menu, "window", 1, 0)], ["Ctrl+Shift+D"],
            _ => dock.Float(dock.ActiveItem),
            _ => dock.ActiveItem is { Location: not DockSide.Floating, Descriptor.CanFloat: true });

        Add(pool, "shell.view.attach", "shell:command.attach", "attach", [new(Menu, "window", 1, 1)], ["Ctrl+Shift+A"],
            _ => dock.Dock(dock.ActiveItem is { Location: DockSide.Floating } active ? active : dock.LastActiveFloating),
            _ => dock.LastActiveFloating is not null || dock.ActiveItem is { Location: DockSide.Floating });

        Add(pool, "shell.document.close", "shell:command.closedocument", "close", [new(Menu, "window", 1, 2)], ["Ctrl+F4"],
            _ => dock.Close(dock.ActiveDocument),
            _ => dock.ActiveDocument is { Descriptor.CanClose: true });

        Add(pool, "shell.document.next", "shell:command.nextdocument", "forward", [new(Menu, "window", 1, 3)], ["Ctrl+F6"],
            _ => dock.NextDocument());

        Add(pool, "shell.document.previous", "shell:command.previousdocument", "back", [new(Menu, "window", 1, 4)], ["Ctrl+Shift+F6"],
            _ => dock.PreviousDocument());

        AddToggle(pool, "shell.pane.left", "shell:command.toggleleft", "dock.left", [new(Menu, "window", 2, 0)],
            _ => dock.TogglePane(DockSide.Left), () => dock.IsPaneVisible(DockSide.Left));
        AddToggle(pool, "shell.pane.right", "shell:command.toggleright", "dock.right", [new(Menu, "window", 2, 1)],
            _ => dock.TogglePane(DockSide.Right), () => dock.IsPaneVisible(DockSide.Right));
        AddToggle(pool, "shell.pane.bottom", "shell:command.togglebottom", "dock.bottom", [new(Menu, "window", 2, 2)],
            _ => dock.TogglePane(DockSide.Bottom), () => dock.IsPaneVisible(DockSide.Bottom));

        Add(pool, "shell.layout.reset", "shell:command.resetlayout", "reset", [new(Menu, "window", 3)], [],
            _ => dock.ResetLayout());

        Add(pool, "shell.language.toggle", "shell:command.language", "language", [new(Menu, "view", 3)], [],
            _ =>
            {
                var next = context.Languages.Current.Equals("de", StringComparison.OrdinalIgnoreCase) ? "en" : "de";
                context.Languages.Set(next);
                context.Settings.Current.Language = next;
                context.Settings.Save();
            });

        Add(pool, "shell.about", "shell:command.about", "info", [new(Menu, "help", 0)], ["F1"],
            _ => mainWindow()?.Shell?.ShowAboutAsync());

        // Enabled state of the window commands depends on the active view.
        dock.ActiveItemChanged += (_, _) => pool.RequeryAll();
        dock.LayoutChanged += (_, _) => pool.RequeryAll();
    }

    private static void Add(RelayCommandPool pool, string id, string textKey, string icon, CommandPlacement[] placements, string[] shortcuts, Action<object?> execute, Func<object?, bool>? canExecute = null) =>
        pool.Register(Module, new CommandDescriptor(id)
        {
            TextKey = textKey,
            Icon = icon,
            Placements = placements,
            DefaultShortcuts = shortcuts.Select(s => ShortcutBinding.Parse(s)).ToList(),
        }, new DelegateCommandHandler(execute, canExecute));

    private static void AddToggle(RelayCommandPool pool, string id, string textKey, string icon, CommandPlacement[] placements, Action<object?> execute, Func<bool> isChecked) =>
        pool.Register(Module, new CommandDescriptor(id)
        {
            TextKey = textKey,
            Icon = icon,
            Kind = CommandKind.Toggle,
            Placements = placements,
        }, new DelegateCommandHandler(execute, null, _ => isChecked()));
}
