// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : RelayCommandPool.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Central command manager of the application. Single source of truth for which commands
//   exist, what state they are in, how they run and where they appear in menus.
//
// RESPONSIBILITIES
//   1. Registration
//      - Register(moduleId, descriptor, handler) / RegisterGroup(moduleId, groupDescriptor),
//        delegated to the DynamicCommandRegistry.
//      - Bulk unregister per module (unload) and atomic replacement per module (hot-swap).
//   2. Lookup
//      - Get(commandId): exactly one stable PooledCommand per command ID (per window).
//      - Instances also exist for IDs that are not registered yet (CanExecute = false),
//        so bindings and menus work before a module has loaded.
//   3. State
//      - Central evaluation of CanExecute, visibility and checked state (toggle commands).
//      - Requery(commandId) / RequeryAll(): raise CanExecuteChanged through the pool's own dispatcher.
//      - Triggers: hot-swap, permission change, module load/unload, explicit state changes.
//   4. Execution
//      - Execute(commandId, parameter) for callers without a binding: keyboard shortcuts,
//        command palette, toast/deep-link activation, scripting.
//      - Always through the CommandExecutor.
//   5. Structure (command trees)
//      - GetTree(placement, context): returns the command tree for a menu location
//        (e.g. main menu, a context menu, the navigation tree) via CommandTreeBuilder.
//      - TreeChanged(placement) event whenever the tree for a placement changes
//        (module loaded/unloaded/swapped, permissions changed, group renamed).
//   6. Search
//      - Find(text): flat list of executable commands with their tree path as breadcrumb
//        ("File > Export > PDF"), for a command palette / AutoSuggestBox.
//   7. UI dispatching
//      - Owns the application's UI dispatcher (Dispatcher property, a CommandDispatcher).
//      - Every UI-facing change - CanExecuteChanged, checked state, TreeChanged, execution
//        results - is marshalled through it to the UI thread of the right window.
//      - The ClientAppContext exposes the same instance as ApplicationContext.Dispatcher,
//        so ViewModels and the log use the command pool's dispatcher too.
//   8. Keyboard shortcuts (ShortcutManager, see ShortcutManager.cs)
//      - Central table of all shortcuts; conflict detection across modules;
//        provides KeyboardAccelerators to the shell window.
//
// RULES
//   - The manager never holds references to module types beyond what the registry holds,
//     so an old module version can be unloaded after a hot-swap.
//   - All public members are thread-safe; UI-facing events are raised on the UI thread.
//   - Permission checks here gate the UI only - the server authorizes again.
//
// WINUI NOTE
//   WinUI has no CommandManager.RequerySuggested; this class is its replacement and must
//   actively trigger every CanExecute update.
//
// RESERVED SHORTCUTS
//   Ctrl+Tab and Ctrl+Shift+Tab belong to the shell's navigation wheel
//   ("shell.navigationwheel.show"); registrations from modules that use them are rejected.
//
// SHORTCUT MANAGER
//   - Shortcuts: the ShortcutManager owned by the pool (like the Dispatcher). Registration of a
//     command forwards its DefaultShortcuts; unregistering a module removes its bindings.
