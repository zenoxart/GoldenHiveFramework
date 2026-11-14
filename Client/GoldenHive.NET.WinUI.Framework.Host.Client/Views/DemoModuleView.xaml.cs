using Microsoft.UI.Xaml.Controls;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Views;

/// <summary>View of the built-in demo modules.</summary>
public sealed partial class DemoModuleView : UserControl
{
    public DemoModuleView(DemoModuleViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }

    public DemoModuleViewModel ViewModel { get; }
}
