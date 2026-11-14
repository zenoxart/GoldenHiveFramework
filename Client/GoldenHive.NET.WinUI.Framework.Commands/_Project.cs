// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : _Project.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PROJECT
//   Class library for the central command manager (RelayCommandPool).
//   TargetFramework net10.0-windows10.0.26100.0.
//
// ROLE
//   The RelayCommandPool is THE command manager of the application. Every command -
//   from the host or from any module - is registered, looked up, executed, enabled/disabled
//   and arranged in menus through it. Nothing in the UI creates ICommand instances of its own.
//   It replaces WPF's CommandManager, which does not exist in WinUI.
//
// COMPONENTS
//   - RelayCommandPool        central command manager (public entry point / facade)
//   - PooledCommand           stable ICommand instance per command ID
//   - DynamicCommandRegistry  current implementation + descriptor per command ID
//   - CommandExecutor         uniform execution (permission, cancellation, errors)
//   - CommandDispatcher       the application's UI dispatcher (one DispatcherQueue per window)
//   - ShortcutManager         effective shortcut table: defaults, presets, overrides, conflicts
//   - ShortcutInputRouter     keyboard input per window -> shortcut -> command
//   - ShortcutProfileStore    presets and user overrides
//   - CommandTreeBuilder      generates command trees for menus from flat registrations
//   - CommandTreeAdapters     turns a command tree into WinUI menu/tree controls
//   - CommandUiDescriptor     label, icon, shortcut -> XamlUICommand
//   - Ui                      attached properties gh:Ui.Command / Icon / Text / ToolTip
//   - RichToolTipBuilder      standard tooltip: label + shortcut + text + disabled reason
//
// EXECUTION CHAIN
//   WinUI control -> ViewModel ICommand -> RelayCommandPool -> DynamicCommandRegistry
//   -> permission check -> CommandExecutor -> module handler -> backend API.
//
// STRUCTURE CHAIN
//   Module registers descriptors with placements -> RelayCommandPool -> CommandTreeBuilder
//   -> CommandTree (immutable) -> CommandTreeAdapters -> MenuBar / MenuFlyout / TreeView /
//   NavigationView / CommandBar.
