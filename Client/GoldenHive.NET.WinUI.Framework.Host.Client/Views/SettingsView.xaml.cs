using GoldenHive.NET.WinUI.Framework.CommandContracts;
using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using VirtualKey = Windows.System.VirtualKey;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Views;

/// <summary>Settings page: language, keyboard shortcuts (record by pressing keys) and data folders.</summary>
public sealed partial class SettingsView : UserControl
{
    public SettingsView(SettingsViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        LanguageBox.SelectedItem = viewModel.CurrentLanguage;
    }

    public SettingsViewModel ViewModel { get; }

    private void OnLanguageChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LanguageBox.SelectedItem is LanguageInfo language)
        {
            ViewModel.SetLanguage(language);
        }
    }

    private void OnChangeShortcut(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: string commandId })
        {
            ViewModel.StartRecording(commandId);
        }
    }

    private void OnResetShortcut(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: string commandId })
        {
            ViewModel.Reset(commandId);
        }
    }

    private void OnResetAll(object sender, RoutedEventArgs e) => ViewModel.ResetAll();

    private void OnOpenLocal(object sender, RoutedEventArgs e) => ViewModel.OpenFolder(ViewModel.LocalDataPath);

    private void OnOpenRoaming(object sender, RoutedEventArgs e) => ViewModel.OpenFolder(ViewModel.RoamingDataPath);

    private void OnPreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (ViewModel.RecordingCommandId is null || ShortcutInputRouter.IsModifierKey(e.Key))
        {
            return;
        }

        e.Handled = true;
        if (e.Key == VirtualKey.Escape)
        {
            ViewModel.FinishRecording(null);
            return;
        }

        ViewModel.FinishRecording(new KeyGesture(ShortcutInputRouter.CurrentModifiers(), (int)e.Key));
    }
}
