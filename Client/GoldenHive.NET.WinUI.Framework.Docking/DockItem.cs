using GoldenHive.NET.WinUI.Framework.ModuleContracts;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GoldenHive.NET.WinUI.Framework.Docking;

/// <summary>
/// One open view in the dock layout (document, tool window or floating window). The ViewModel stays the
/// same when the item moves between panes and windows; the view element is recreated per host, because
/// XAML elements cannot move between windows.
/// </summary>
public sealed class DockItem
{
    /// <summary>Unique key of the open view (navigation key, plus parameter for multi-instance views).</summary>
    public required string Key { get; init; }

    public required string NavigationKey { get; init; }

    public required string ModuleId { get; init; }

    public string? Parameter { get; init; }

    public required ViewDescriptor Descriptor { get; init; }

    public required ViewModelAppObject ViewModel { get; init; }

    /// <summary>Creates a new view element for the ViewModel.</summary>
    public required Func<FrameworkElement> CreateView { get; init; }

    public string? Icon { get; init; }

    /// <summary>Text key of the title; when set, the title follows the language.</summary>
    public string? TitleKey { get; init; }

    public string Title => ViewModel.Title;

    /// <summary>Where the item currently is.</summary>
    public DockSide Location { get; internal set; } = DockSide.DocumentWell;

    /// <summary>Where it was docked last (used when a floating item is docked back).</summary>
    public DockSide LastDockedSide { get; internal set; } = DockSide.DocumentWell;

    internal FrameworkElement? View { get; set; }

    internal TabViewItem? Tab { get; set; }

    internal FloatingDockWindow? FloatingWindow { get; set; }
}
