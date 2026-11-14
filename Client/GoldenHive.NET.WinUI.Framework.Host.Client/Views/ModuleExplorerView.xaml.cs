using GoldenHive.NET.WinUI.Framework.CommandContracts;
using Microsoft.UI.Xaml.Controls;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Views;

/// <summary>Entry of the Module Explorer tree (the TreeView shows its label).</summary>
public sealed record ExplorerEntry(CommandTreeNode Node, string Label)
{
    public override string ToString() => Label;
}

/// <summary>The Module Explorer tool window: modules and their views as a tree (command tree "shell.navigation").</summary>
public sealed partial class ModuleExplorerView : UserControl
{
    public ModuleExplorerView(ModuleExplorerViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        Build();
        Loaded += (_, _) => viewModel.TreeChanged += OnTreeChanged;
        Unloaded += (_, _) => viewModel.TreeChanged -= OnTreeChanged;
    }

    public ModuleExplorerViewModel ViewModel { get; }

    private void OnTreeChanged(object? sender, EventArgs e) => Build();

    private void Build()
    {
        Tree.RootNodes.Clear();
        foreach (var node in ViewModel.Tree)
        {
            if (CreateNode(node) is { } treeNode)
            {
                Tree.RootNodes.Add(treeNode);
            }
        }
    }

    private TreeViewNode? CreateNode(CommandTreeNode node)
    {
        if (node.Kind == CommandTreeNodeKind.Separator)
        {
            return null;
        }

        var treeNode = new TreeViewNode { Content = new ExplorerEntry(node, ViewModel.Label(node)), IsExpanded = true };
        foreach (var child in node.Children)
        {
            if (CreateNode(child) is { } childNode)
            {
                treeNode.Children.Add(childNode);
            }
        }

        return treeNode;
    }

    private void OnItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
    {
        if (args.InvokedItem is TreeViewNode { Content: ExplorerEntry entry })
        {
            ViewModel.Execute(entry.Node);
        }
    }
}
