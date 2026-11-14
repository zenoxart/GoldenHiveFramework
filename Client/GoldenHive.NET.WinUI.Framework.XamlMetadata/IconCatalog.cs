using System.Collections.Concurrent;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace GoldenHive.NET.WinUI.Framework.XamlMetadata;

/// <summary>
/// Resolves icon references (IconRef) to WinUI icons. References:
/// "save" (shell catalogue), "documents.invoice" (module icon), "glyph:E74E" (direct glyph),
/// "image:Assets/x.svg" (file in the package). Glyphs use the system symbol font
/// (Segoe Fluent Icons on Windows 11, Segoe MDL2 Assets on Windows 10) and follow the
/// control's foreground, so they adapt to theme, hover, disabled and high contrast.
/// </summary>
public sealed class IconCatalog : AppObject
{
    private const string PlaceholderGlyph = "";

    private static readonly Dictionary<string, string> DefaultGlyphs = new(StringComparer.OrdinalIgnoreCase)
    {
        ["add"] = "",
        ["apps"] = "",
        ["attach"] = "",
        ["back"] = "",
        ["clear"] = "",
        ["close"] = "",
        ["delete"] = "",
        ["detach"] = "",
        ["document"] = "",
        ["dock.bottom"] = "",
        ["dock.left"] = "",
        ["dock.right"] = "",
        ["edit"] = "",
        ["error"] = "",
        ["folder"] = "",
        ["forward"] = "",
        ["fullscreen"] = "",
        ["help"] = "",
        ["home"] = "",
        ["info"] = "",
        ["keyboard"] = "",
        ["language"] = "",
        ["library"] = "",
        ["list"] = "",
        ["more"] = "",
        ["output"] = "",
        ["pin"] = "",
        ["power"] = "",
        ["refresh"] = "",
        ["reset"] = "",
        ["save"] = "",
        ["search"] = "",
        ["settings"] = "",
        ["switch"] = "",
        ["unpin"] = "",
        ["view"] = "",
        ["warning"] = "",
    };

    private readonly ConcurrentDictionary<string, string> _glyphs = new(DefaultGlyphs, StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, Uri> _images = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, byte> _reportedMissing = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Registers or replaces a glyph icon, e.g. RegisterGlyph("documents.invoice", "").</summary>
    public void RegisterGlyph(string name, string glyph) => _glyphs[name] = glyph;

    /// <summary>Registers an image icon (SVG or PNG), e.g. ms-appx:///Assets/Icons/invoice.svg.</summary>
    public void RegisterImage(string name, Uri image) => _images[name] = image;

    /// <summary>Removes all icons whose name starts with the prefix (e.g. a module ID when it unloads).</summary>
    public void UnregisterPrefix(string prefix)
    {
        foreach (var key in _glyphs.Keys.Where(k => k.StartsWith(prefix + ".", StringComparison.OrdinalIgnoreCase)))
        {
            _glyphs.TryRemove(key, out _);
        }

        foreach (var key in _images.Keys.Where(k => k.StartsWith(prefix + ".", StringComparison.OrdinalIgnoreCase)))
        {
            _images.TryRemove(key, out _);
        }
    }

    /// <summary>Creates an icon element (for AppBarButton.Icon, MenuFlyoutItem.Icon, content).</summary>
    public IconElement CreateIcon(string? iconRef) =>
        TryGetImage(iconRef, out var image)
            ? new ImageIcon { Source = CreateImageSource(image) }
            : new FontIcon { Glyph = ResolveGlyph(iconRef) };

    /// <summary>Creates an icon source (for TabViewItem.IconSource, TitleBar, InfoBar).</summary>
    public IconSource CreateIconSource(string? iconRef) =>
        TryGetImage(iconRef, out var image)
            ? new ImageIconSource { ImageSource = CreateImageSource(image) }
            : new FontIconSource { Glyph = ResolveGlyph(iconRef) };

    /// <summary>Returns the glyph for a reference, or a neutral placeholder when it is unknown.</summary>
    public string ResolveGlyph(string? iconRef)
    {
        if (string.IsNullOrWhiteSpace(iconRef))
        {
            return PlaceholderGlyph;
        }

        if (iconRef.StartsWith("glyph:", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                return char.ConvertFromUtf32(Convert.ToInt32(iconRef[6..], 16));
            }
            catch (Exception)
            {
                return PlaceholderGlyph;
            }
        }

        if (_glyphs.TryGetValue(iconRef, out var glyph))
        {
            return glyph;
        }

        if (_reportedMissing.TryAdd(iconRef, 0) && HasAppContext)
        {
            AppContext.Log.Write($"Unknown icon '{iconRef}'", LogEntryType.Warning, nameof(IconCatalog));
        }

        return PlaceholderGlyph;
    }

    private static ImageSource CreateImageSource(Uri image) =>
        image.AbsolutePath.EndsWith(".svg", StringComparison.OrdinalIgnoreCase)
            ? new SvgImageSource(image)
            : new BitmapImage(image);

    private bool TryGetImage(string? iconRef, out Uri image)
    {
        image = null!;
        if (string.IsNullOrWhiteSpace(iconRef))
        {
            return false;
        }

        if (iconRef.StartsWith("image:", StringComparison.OrdinalIgnoreCase))
        {
            image = new Uri("ms-appx:///" + iconRef[6..].TrimStart('/'));
            return true;
        }

        if (_images.TryGetValue(iconRef, out var registered))
        {
            image = registered;
            return true;
        }

        return false;
    }
}
