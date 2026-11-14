using Microsoft.UI.Xaml.Controls;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Views;

/// <summary>The start page: welcome, quick actions, modules and tips.</summary>
public sealed partial class StartView : UserControl
{
    public StartView(StartViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }

    public StartViewModel ViewModel { get; }

    private void OnModuleClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is ModuleCard card)
        {
            ViewModel.Open(card);
        }
    }
}
