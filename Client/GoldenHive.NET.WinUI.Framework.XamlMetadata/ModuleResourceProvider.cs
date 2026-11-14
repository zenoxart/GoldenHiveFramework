// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.XamlMetadata
// File    : ModuleResourceProvider.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Provides a module's localized strings and images.
//
// BEHAVIOUR
//   - Either the module's own .pri file via the MRT Core ResourceManager with a path,
//     or module-owned RESX/JSON resources.
//   - Images as file paths in the module folder (ms-appx does not point to module folders).
//   - Take the language from the host's settings.
//
// TEXTS AND ICONS
//   - Module texts: per language in the package (e.g. Strings/de/strings.json or .resw compiled to
//     the module's .pri), registered with the LanguageManager under the module ID scope.
//   - Module icons: SVG/PNG files and glyph mappings registered with the IconCatalog.
