using GoldenHive.NET.WinUI.Framework.CommandContracts;
using GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;
using GoldenHive.NET.WinUI.Framework.Mvvm;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Shell;

/// <summary>Main ViewModel of the shell: status bar texts and the active view.</summary>
public sealed class ShellViewModel : ViewModelAppObject
{
    private string _statusText = string.Empty;
    private string _activeTitle = string.Empty;
    private string _sizeClassText = string.Empty;
    private string _languageText = string.Empty;
    private string? _chordHint;

    public string StatusText
    {
        get => _statusText;
        private set => SetProperty(ref _statusText, value);
    }

    public string ActiveTitle
    {
        get => _activeTitle;
        private set => SetProperty(ref _activeTitle, value);
    }

    public string SizeClassText
    {
        get => _sizeClassText;
        private set => SetProperty(ref _sizeClassText, value);
    }

    public string LanguageText
    {
        get => _languageText;
        private set => SetProperty(ref _languageText, value);
    }

    private ClientAppContext Client => Context<ClientAppContext>();

    /// <summary>Shows the "waiting for the second key" hint of a chord (null hides it).</summary>
    public void SetChordHint(string? gesture)
    {
        _chordHint = gesture;
        UpdateStatus();
    }

    protected override void OnCreated()
    {
        UpdateTexts();
        AppContext.Languages.LanguageChanged += (_, _) => UpdateTexts();
        Client.Dock.ActiveItemChanged += (_, _) => OnActiveItemChanged();
    }

    protected override void OnSizeClassChanged() =>
        SizeClassText = AppContext.Languages.GetText($"shell:sizeclass.{SizeClass.ToString().ToLowerInvariant()}");

    private void UpdateTexts()
    {
        UpdateStatus();
        OnSizeClassChanged();
        LanguageText = AppContext.Languages.CurrentInfo.NativeName;
        OnActiveItemChanged();
    }

    private void UpdateStatus() =>
        StatusText = _chordHint is null
            ? AppContext.Languages.GetText("shell:status.ready")
            : AppContext.Languages.GetText("shell:status.chord", new Dictionary<string, object?> { ["gesture"] = _chordHint });

    private void OnActiveItemChanged()
    {
        var item = Client.Dock.ActiveItem;
        ActiveTitle = item?.Title ?? string.Empty;
        Client.Commands.Shortcuts.ActiveScopes = item is null
            ? []
            : [(ShortcutScope.View, item.NavigationKey), (ShortcutScope.Module, item.ModuleId)];
    }
}
