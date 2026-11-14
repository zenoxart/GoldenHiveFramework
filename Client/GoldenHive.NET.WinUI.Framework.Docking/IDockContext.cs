namespace GoldenHive.NET.WinUI.Framework.Docking;

/// <summary>Implemented by application contexts that provide the dock layout.</summary>
public interface IDockContext
{
    DockManager Dock { get; }
}
