// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Host.Client
// File    : ThemeResourceKeys.cs
// Layer   : Client / Host
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Central list of resource keys that modules are allowed to use.
//
// RULES
//   - Styles, colours, spacing and templates live only in the host (App.xaml,
//     XamlControlsResources, custom ResourceDictionaries).
//   - Modules reference only these keys (via Framework.Contracts), never global styles of
//     their own - this keeps light/dark/high-contrast themes and Fluent design consistent.
//
// RESPONSIVE TOKENS
//   Breakpoints (640 / 1008 / 1920 epx), spacing scale per size class, max content width,
//   minimum touch target size - modules use these keys instead of numbers.
//
// ICON AND TOOLTIP TOKENS
//   Icon sizes (16 / 20 / 24 / 32 epx), icon-to-label spacing, tooltip max width and typography.
