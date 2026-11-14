namespace GoldenHive.NET.WinUI.Framework.Windowing;

/// <summary>Implemented by application contexts that provide window management.</summary>
public interface IWindowContext
{
    WindowManager Windows { get; }
}
