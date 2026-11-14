using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;
using GoldenHive.NET.WinUI.Framework.Host.Client.Shell;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GoldenHive.NET.WinUI.Framework.Host.Client;

/// <summary>
/// The main window: Golden Hive title bar with the command palette (Ctrl+Shift+P) and the shell.
/// Its placement is saved per monitor in AppData; closing it saves the dock layout and ends the app.
/// </summary>
public sealed partial class MainWindow : Window
{
    private readonly ClientAppContext _context;

    public MainWindow(ClientAppContext context)
    {
        _context = context;
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        AppWindow.SetIcon("Assets/GoldenHive.ico");

        Shell = new ShellView(context, this);
        ShellHost.Child = Shell;
        UpdateTexts();
        context.Languages.LanguageChanged += (_, _) => UpdateTexts();

        context.Windows.Track(this, "main", 480, 400, isMain: true);
        Activated += (_, e) => IsActive = e.WindowActivationState != WindowActivationState.Deactivated;
        Closed += OnClosed;
    }

    public ShellView Shell { get; }

    /// <summary>Puts the profile of the signed-in user into the right side of the title bar.</summary>
    public void AttachProfile(Func<Task> signIn) => AppTitleBar.RightHeader = new ProfileButton(_context, this, signIn);

    /// <summary>True while the window has the focus (toasts are only shown when it has not).</summary>
    public bool IsActive { get; private set; }

    /// <summary>Runs <paramref name="action"/> once the content is loaded (XamlRoot available for dialogs).</summary>
    public void WhenLoaded(Func<Task> action)
    {
        var root = (FrameworkElement)Content;
        if (root.IsLoaded)
        {
            DispatcherQueue.TryEnqueue(async () => await action());
            return;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            root.Loaded -= OnLoaded;
            DispatcherQueue.TryEnqueue(async () => await action());
        }

        root.Loaded += OnLoaded;
    }

    /// <summary>Focuses the command palette and shows all commands.</summary>
    public void FocusPalette()
    {
        PaletteBox.Text = string.Empty;
        PaletteBox.ItemsSource = _context.Commands.Find(string.Empty);
        PaletteBox.Focus(FocusState.Programmatic);
        PaletteBox.IsSuggestionListOpen = true;
    }

    private void UpdateTexts()
    {
        var title = _context.Languages.GetText("shell:app.title");
        Title = title;
        AppTitleBar.Title = title;
        PaletteBox.PlaceholderText = _context.Languages.GetText("shell:palette.placeholder");
    }

    private void OnPaletteTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            sender.ItemsSource = _context.Commands.Find(sender.Text);
        }
    }

    private void OnPaletteQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        var result = args.ChosenSuggestion as CommandSearchResult
            ?? (sender.ItemsSource as IReadOnlyList<CommandSearchResult>)?.FirstOrDefault(r => r.IsEnabled);
        sender.Text = string.Empty;
        sender.ItemsSource = null;
        if (result is not null)
        {
            _ = _context.Commands.ExecuteAsync(result.CommandId);
        }
    }

    private void OnClosed(object sender, WindowEventArgs args)
    {
        _context.Hub.Stop();
        _context.HotSwap.Stop();
        _context.Toasts.Unregister();
        _context.Dock.Shutdown();
        _context.Windows.Placements.Save();
        _context.Log.Write("Golden Hive ends", LogEntryType.End, nameof(MainWindow));
    }
}
