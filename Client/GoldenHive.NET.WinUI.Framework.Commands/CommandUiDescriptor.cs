// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : CommandUiDescriptor.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Maps command metadata onto WinUI.
//
// CONTENTS
//   - Label, description, icon (symbol or glyph), keyboard shortcut from CommandDescriptor.
//   - Creates a XamlUICommand pointing to the PooledCommand, for
//     MenuFlyoutItem, AppBarButton and KeyboardAccelerator.
//   - Localized text from the module's resources (ModuleResourceProvider).
//
// ICONS AND LOCALIZATION
//   - Icon via IconCatalog (IconRef), label/tooltip/description via the LanguageManager using the
//     command's TextKey conventions; tooltip assembled by the RichToolTipBuilder.
//   - The XamlUICommand is updated live when the language, theme or shortcut changes.
