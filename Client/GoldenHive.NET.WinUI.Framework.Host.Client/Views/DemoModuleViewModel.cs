using System.Collections.ObjectModel;
using GoldenHive.NET.WinUI.Framework.Mvvm;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Views;

/// <summary>ViewModel of the built-in demo modules.</summary>
public sealed class DemoModuleViewModel : ViewModelAppObject
{
    private string _description = string.Empty;

    public string ModuleKey { get; private set; } = string.Empty;

    public string Description
    {
        get => _description;
        private set => SetProperty(ref _description, value);
    }

    public ObservableCollection<string> Items { get; } = [];

    public void Initialize(string moduleKey)
    {
        ModuleKey = moduleKey;
        Refresh();
        AppContext.Languages.LanguageChanged += (_, _) => Refresh();
        for (var i = 1; i <= 25; i++)
        {
            Items.Add($"{moduleKey} #{i:000}");
        }
    }

    private void Refresh() => Description = AppContext.Languages.GetText("shell:" + ModuleKey + ".description");
}
