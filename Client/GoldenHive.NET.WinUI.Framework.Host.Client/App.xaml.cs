using System.Globalization;
using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using GoldenHive.NET.WinUI.Framework.Workflows;
using Microsoft.UI.Xaml;

namespace GoldenHive.NET.WinUI.Framework.Host.Client;

/// <summary>
/// Golden Hive host application. Creates the client application context, registers texts, icons, shell commands,
/// built-in and workflow views, opens the main window and then runs the startup sequence (sign-in, modules,
/// notification stream).
/// </summary>
public partial class App : Application
{
    private MainWindow? _window;
    private StartupSequence? _startup;

    public App()
    {
        InitializeComponent();
        UnhandledException += (_, e) =>
        {
            Context.Log.Write($"Unhandled exception: {e.Message}", LogEntryType.Error, nameof(App), e.Exception);
            e.Handled = true;
        };
    }

    /// <summary>The root application context (WIFI: AppKontext of the client).</summary>
    public static ClientAppContext Context { get; } = new();

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        // Application resources can only be changed once XAML is running, not in the App constructor.
        Theme.GoldenHiveTheme.Apply(Resources);

        var context = Context;
        context.Dispatcher = context.Commands.Dispatcher;
        HostTexts.Register(context.Languages);
        PlatformTexts.Register(context.Languages);
        WorkflowTexts.Register(context.Languages);
        context.Languages.Set(context.Settings.Current.Language ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
        context.Log.Write("Golden Hive starts", LogEntryType.Start, nameof(App));

        RegisterIcons(context);
        Ui.Initialize(context.Commands);

        // UI gating only - every service authorizes again on the server.
        context.Commands.Executor.PermissionCheck = permission => context.Backend.Has(permission);

        ShellCommands.Register(context, () => _window, () => _startup);
        BuiltInViews.Register(context);

        _window = new MainWindow(context);
        _startup = new StartupSequence(context, _window);
        _startup.Status += (_, text) => _window.Shell.ViewModel.SetStartupStatus(text);
        _window.AttachProfile(() => _startup.SignInAsync());
        _window.Activate();
        _window.WhenLoaded(async () =>
        {
            try
            {
                await _startup.RunAsync();
            }
            catch (Exception ex)
            {
                context.Log.Write($"Startup failed: {ex.Message}", LogEntryType.Error, nameof(App), ex);
            }
        });
    }

    private static void RegisterIcons(ClientAppContext context)
    {
        var icons = context.Icons;
        icons.RegisterGlyph("notifications", "\uEA8F");
        icons.RegisterGlyph("signin", "\uE77B");
        icons.RegisterGlyph("signout", "\uF3B1");
        icons.RegisterGlyph("sync", "\uE895");
        icons.RegisterGlyph("workflow", "\uE9D5");
        icons.RegisterGlyph("workflow.inbox", "\uE715");
        icons.RegisterGlyph("workflow.impact", "\uE7BA");
        icons.RegisterGlyph("workflow.ai", "\uE99A");
    }
}
