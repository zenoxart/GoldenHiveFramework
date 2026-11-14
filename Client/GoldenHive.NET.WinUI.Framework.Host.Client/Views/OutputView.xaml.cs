using System.Collections.Specialized;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Views;

/// <summary>The Output tool window: the live application log.</summary>
public sealed partial class OutputView : UserControl
{
    public OutputView(OutputViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        AutoScrollBox.IsChecked = viewModel.AutoScroll;
        Loaded += (_, _) =>
        {
            viewModel.Entries.CollectionChanged += OnEntriesChanged;
            ScrollToEnd();
        };
        Unloaded += (_, _) => viewModel.Entries.CollectionChanged -= OnEntriesChanged;
    }

    public OutputViewModel ViewModel { get; }

    private void OnEntriesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (ViewModel.AutoScroll && e.Action == NotifyCollectionChangedAction.Add)
        {
            ScrollToEnd();
        }
    }

    private void ScrollToEnd()
    {
        if (ViewModel.Entries.Count > 0)
        {
            LogList.ScrollIntoView(ViewModel.Entries[^1]);
        }
    }

    private void OnClear(object sender, RoutedEventArgs e) => ViewModel.Clear();

    private void OnAutoScrollChanged(object sender, RoutedEventArgs e) => ViewModel.AutoScroll = AutoScrollBox.IsChecked == true;
}
