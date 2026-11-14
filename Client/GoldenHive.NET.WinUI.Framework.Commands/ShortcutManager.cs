// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : ShortcutManager.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The shortcut manager of the application. Lives inside the RelayCommandPool
//   (RelayCommandPool.Shortcuts) - every shortcut is bound to a command ID, so commands,
//   their state and their shortcuts are managed in one place.
//
// LAYERS (later layer wins)
//   1. Shell defaults   : reserved and standard shortcuts of the host (see below).
//   2. Module defaults  : DefaultShortcuts of the modules' command descriptors.
//   3. Preset           : selectable profile ("Golden Hive default", "Visual Studio-like", ...).
//   4. Tenant overrides : from the Configuration Service (admins can enforce or block bindings).
//   5. User overrides   : the user's own changes (roaming, synced via the Configuration Service).
//   The result is the effective binding table: gesture + scope -> command ID.
//
// RESPONSIBILITIES
//   - Build and rebuild the effective table when modules load/unload/hot-swap, permissions change
//     or settings change. User overrides are keyed by command ID, so they survive module updates;
//     overrides for commands that no longer exist are kept but shown as "orphaned".
//   - Conflict detection: same gesture in the same or an overlapping scope.
//       * module vs. reserved shell shortcut -> module binding rejected, logged;
//       * module vs. module                  -> first registered wins, conflict listed in the
//                                               settings page for the user to resolve;
//       * user override                      -> the user is asked to replace the other binding.
//   - Resolution at key press: find the binding for the gesture in the most specific active scope;
//     only enabled, permitted commands (CanExecute) are executed - otherwise the key is passed on.
//   - Provide display text for every command's shortcut: menus, tooltips, command palette,
//     navigation wheel and cheat sheet all show the effective, localized gesture.
//   - Import / export of user bindings as JSON; reset per command, per module or all.
//
// RESERVED SHELL SHORTCUTS (only the user may remap them, never modules)
//   Ctrl+Tab / Ctrl+Shift+Tab   navigation wheel (hold gesture)
//   Ctrl+Shift+P                command palette
//   Ctrl+K, Ctrl+S              shortcut settings (chord)
//   Ctrl+/ (hold ~1 s on Ctrl)  shortcut cheat sheet for the current context
//   F1                          help for the current view
//   Esc                         close overlay / cancel
//
// EVENTS
//   BindingsChanged -> the ShortcutInputRouter regenerates the windows' accelerators and the
//   command tree adapters refresh the shortcut text in menus.
//
// WINDOW SHORTCUTS (shell defaults, not reserved)
//   Ctrl+Shift+D  detach current view      Ctrl+Shift+A  attach view back to main window
//
// DOCK SHORTCUTS (shell defaults, Visual Studio style, remappable)
//   Ctrl+F4 close document         Ctrl+F6 / Ctrl+Shift+F6 next / previous document
//   Shift+Esc hide tool window     Alt+-   pane menu (dock, float, auto-hide)
//   Alt+Shift+Enter full screen    Ctrl+- / Ctrl+Shift+- navigate backward / forward
//   Ctrl+Tab stays the navigation wheel (which also lists open documents and tool windows).
