using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;
using GoldenHive.NET.WinUI.Framework.Navigation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Shell;

/// <summary>
/// The shell of the main window: menu bar (command tree "shell.mainmenu"), dock layout, status bar and the
/// overlay with the navigation wheel. Responsive: the dock layout follows the shell's size class.
/// </summary>
public sealed partial class ShellView : UserControl
{
    private const string MainMenuPlacement = "shell.mainmenu";
    private readonly ClientAppContext _context;
    private readonly NavigationWheelViewModel _wheelViewModel;
    private readonly NavigationWheelInputHandler _wheelInput;

    public ShellView(ClientAppContext context, Window window)
    {
        _context = context;
        ViewModel = context.Produce<ShellViewModel>();
        InitializeComponent();

        DockArea.Child = context.Dock.CreateHost();
        BuildMenu();
        context.Commands.TreeChanged += (_, placement) =>
        {
            if (placement is "*" or MainMenuPlacement)
            {
                BuildMenu();
            }
        };

        context.Windows.Responsive.Attach(Root, (sizeClass, heightClass) =>
        {
            ViewModel.SizeClass = sizeClass;
            ViewModel.HeightClass = heightClass;
            context.Dock.ApplySizeClass(sizeClass);
        });

        _wheelViewModel = context.Produce<NavigationWheelViewModel>();
        Overlay.Children.Add(new NavigationWheel(_wheelViewModel, context.Icons));
        _wheelInput = new NavigationWheelInputHandler(
            _wheelViewModel,
            (UIElement)window.Content,
            DispatcherQueue,
            visible => Overlay.Visibility = visible ? Visibility.Visible : Visibility.Collapsed);
        _wheelViewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NavigationWheelViewModel.IsOpen))
            {
                context.Commands.InputRouter.IsSuspended = _wheelViewModel.IsOpen;
                if (!_wheelViewModel.IsOpen)
                {
                    Overlay.Visibility = Visibility.Collapsed;
                }
            }
        };
        Overlay.Tapped += (_, e) =>
        {
            if (ReferenceEquals(e.OriginalSource, Overlay))
            {
                _wheelInput.Cancel();
            }
        };
        window.Activated += (_, e) =>
        {
            if (e.WindowActivationState == WindowActivationState.Deactivated && _wheelViewModel.IsOpen)
            {
                _wheelInput.Cancel();
            }
        };

        context.Commands.InputRouter.ChordPending += (_, gesture) => ViewModel.SetChordHint(gesture);
    }

    public ShellViewModel ViewModel { get; }

    /// <summary>Starts the navigation wheel (Ctrl+Tab or menu).</summary>
    public void BeginWheel(bool reverse) => _wheelInput.Begin(reverse);

    public async Task ShowAboutAsync()
    {
        var languages = _context.Languages;
        var dialog = new ContentDialog
        {
            XamlRoot = XamlRoot,
            Title = languages.GetText("shell:about.title"),
            Content = new TextBlock
            {
                Text = languages.GetText("shell:about.text", new Dictionary<string, object?> { ["version"] = _context.Version }),
                TextWrapping = TextWrapping.Wrap,
            },
            CloseButtonText = languages.GetText("shell:about.close"),
            DefaultButton = ContentDialogButton.Close,
        };
        await dialog.ShowAsync();
    }

    private void BuildMenu() =>
        CommandTreeAdapters.FillMenuBar(MainMenu, _context.Commands, _context.Commands.GetTree(MainMenuPlacement));
}
