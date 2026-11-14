using GoldenHive.NET.WinUI.Framework.CommandContracts;
using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using GoldenHive.NET.WinUI.Framework.Navigation;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Views;

/// <summary>ViewModel of the Module Explorer: the command tree of placement "shell.navigation".</summary>
public sealed class ModuleExplorerViewModel : ViewModelAppObject
{
    /// <summary>Raised when the tree must be rebuilt.</summary>
    public event EventHandler? TreeChanged;

    public RelayCommandPool Commands => Context<ClientAppContext>().Commands;

    public IReadOnlyList<CommandTreeNode> Tree => Commands.GetTree(NavigationRegistry.NavigationPlacement);

    public string Label(CommandTreeNode node) => CommandTreeAdapters.Label(Commands, node);

    public void Execute(CommandTreeNode node)
    {
        if (node.CommandId is not null)
        {
            _ = Commands.ExecuteAsync(node.CommandId);
        }
    }

    protected override void OnCreated() =>
        Commands.TreeChanged += (_, placement) =>
        {
            if (placement is "*" or NavigationRegistry.NavigationPlacement)
            {
                TreeChanged?.Invoke(this, EventArgs.Empty);
            }
        };
}
