namespace GoldenHive.NET.WinUI.Framework.Navigation;

/// <summary>Implemented by application contexts that provide navigation.</summary>
public interface INavigationContext
{
    NavigationService Navigation { get; }
}
