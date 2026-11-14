// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.XamlMetadata
// File    : IconCatalog.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Resolves an IconRef to a WinUI IconSource / IconElement. An AppObject (Context.Icons).
//
// SOURCES
//   - Shell catalogue: symbolic names mapped to Segoe Fluent Icons glyphs (Windows 11),
//     with Segoe MDL2 Assets fallback on Windows 10; Golden Hive brand icons as SVG.
//   - Module icons: registered by modules (IModuleContext.RegisterIcon(name, source)) as glyphs,
//     SVG (SvgImageSource) or PNG with theme variants (light / dark / high contrast).
//   - "glyph:" and "image:" references resolved directly.
//
// BEHAVIOUR
//   - Monochrome icons use the control's Foreground, so they follow theme, hover, pressed,
//     disabled and high-contrast colours automatically.
//   - Unknown names: a neutral placeholder icon + a log entry (never an exception).
//   - Cached per name and theme; module icons are dropped when the module unloads.
//   - Icon browser (developer tool window) lists all names with a preview and copy button.
