using System.Globalization;
using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using Microsoft.UI.Xaml;

namespace GoldenHive.NET.WinUI.Framework.Host.Client;

/// <summary>
/// Golden Hive host application. Creates the client application context, registers texts, shell commands
/// and built-in views, then opens the main window.
/// </summary>
public partial class App : Application
{
    private MainWindow? _window;

    public App()
    {
        InitializeComponent();
        UnhandledException += (_, e) =>
            Context.Log.Write($"Unhandled exception: {e.Message}", LogEntryType.Error, nameof(App), e.Exception);
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
        context.Languages.Set(context.Settings.Current.Language ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
        context.Log.Write("Golden Hive starts", LogEntryType.Start, nameof(App));

        Ui.Initialize(context.Commands);
        ShellCommands.Register(context, () => _window);
        BuiltInViews.Register(context);

        _window = new MainWindow(context);
        _window.Activate();
    }
}
