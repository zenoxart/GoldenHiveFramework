// =============================================================================
// Project : GoldenHive.NET.Modules.Reporting.Client
// File    : _Views.cs
// Layer   : Module Reporting
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The module's views: report catalogue, parameter dialog, result view.
//
// RULES (viewMode looseXaml)
//   - XAML files as embedded resources, no code-behind.
//   - {Binding} only, no x:Bind (not compiled when loaded with XamlReader.Load).
//   - Styles/resources only via the host's ThemeResourceKeys.
//
// RESPONSIVE AND DETACHABLE
//   - Every view supports the size classes Compact / Medium / Expanded / Wide through
//     visual states; no fixed sizes; tested from 320 epx width to full screen.
//   - Views must work in a detached window: no assumptions about the main window, navigation
//     parameters serializable for session restore.
//
// ICONS, TEXTS, TOOLTIPS
//   - Buttons bound to commands use gh:Ui.Command="<command id>" - icon, label, tooltip and shortcut
//     come from the command; other buttons use gh:Ui.Icon / gh:Ui.Text / gh:Ui.ToolTip.
//   - No hard-coded visible text; every text is a TextKey of this module.
