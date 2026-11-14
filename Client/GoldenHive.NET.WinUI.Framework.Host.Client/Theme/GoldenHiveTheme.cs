using Microsoft.UI;
using Microsoft.UI.Xaml;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Theme;

/// <summary>
/// Golden Hive accent colours: the system accent is replaced by honeycomb gold.
/// Set in code because the XAML compiler does not resolve &lt;Color&gt; in standalone resource dictionaries.
/// </summary>
public static class GoldenHiveTheme
{
    /// <summary>Writes the gold accent palette into the application resources. Call from OnLaunched before the first window is created (not from the App constructor).</summary>
    public static void Apply(ResourceDictionary resources)
    {
        resources["SystemAccentColor"] = ColorHelper.FromArgb(0xFF, 0xF5, 0xB8, 0x2A);
        resources["SystemAccentColorLight1"] = ColorHelper.FromArgb(0xFF, 0xF7, 0xC5, 0x52);
        resources["SystemAccentColorLight2"] = ColorHelper.FromArgb(0xFF, 0xFA, 0xD4, 0x7C);
        resources["SystemAccentColorLight3"] = ColorHelper.FromArgb(0xFF, 0xFC, 0xE3, 0xA6);
        resources["SystemAccentColorDark1"] = ColorHelper.FromArgb(0xFF, 0xC0, 0x82, 0x0F);
        resources["SystemAccentColorDark2"] = ColorHelper.FromArgb(0xFF, 0x99, 0x67, 0x08);
        resources["SystemAccentColorDark3"] = ColorHelper.FromArgb(0xFF, 0x6E, 0x4A, 0x05);
    }
}
