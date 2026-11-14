using GoldenHive.NET.WinUI.Framework.CommandContracts;
using Microsoft.UI.Xaml.Controls;

namespace GoldenHive.NET.WinUI.Framework.Commands;

/// <summary>Projects command trees onto WinUI menus. Items get icon, label, tooltip and shortcut from their command.</summary>
public static class CommandTreeAdapters
{
    /// <summary>Fills a MenuBar from a tree: root groups become MenuBarItems.</summary>
    public static void FillMenuBar(MenuBar menuBar, RelayCommandPool pool, IReadOnlyList<CommandTreeNode> nodes)
    {
        menuBar.Items.Clear();
        foreach (var group in nodes.Where(n => n.Kind == CommandTreeNodeKind.Group))
        {
            var item = new MenuBarItem { Title = Label(pool, group) };
            foreach (var child in CreateFlyoutItems(pool, group.Children))
            {
                item.Items.Add(child);
            }

            menuBar.Items.Add(item);
        }
    }

    /// <summary>Creates flyout items (MenuFlyout, MenuBarItem, context menus) for a list of nodes.</summary>
    public static IEnumerable<MenuFlyoutItemBase> CreateFlyoutItems(RelayCommandPool pool, IReadOnlyList<CommandTreeNode> nodes)
    {
        foreach (var node in nodes)
        {
            switch (node.Kind)
            {
                case CommandTreeNodeKind.Separator:
                    yield return new MenuFlyoutSeparator();
                    break;
                case CommandTreeNodeKind.Group:
                    var sub = new MenuFlyoutSubItem { Text = Label(pool, node) };
                    if (node.Icon is not null)
                    {
                        Ui.SetIcon(sub, node.Icon);
                    }

                    foreach (var child in CreateFlyoutItems(pool, node.Children))
                    {
                        sub.Items.Add(child);
                    }

                    yield return sub;
                    break;
                default:
                    MenuFlyoutItem item = node.CommandKind == CommandKind.Toggle
                        ? new ToggleMenuFlyoutItem { IsChecked = pool.IsChecked(node.CommandId!, null) == true }
                        : new MenuFlyoutItem();
                    Ui.SetCommand(item, node.CommandId);
                    yield return item;
                    break;
            }
        }
    }

    /// <summary>Localized label of a tree node.</summary>
    public static string Label(RelayCommandPool pool, CommandTreeNode node)
    {
        if (node.CommandId is not null)
        {
            return pool.GetLabel(node.CommandId);
        }

        return node.TextKey is null ? node.Id : pool.GetText(node.TextKey, "label") ?? pool.AppContext.Languages.GetText(node.TextKey + ".label");
    }
}
