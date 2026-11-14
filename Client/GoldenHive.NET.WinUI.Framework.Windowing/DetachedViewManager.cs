// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Windowing
// File    : DetachedViewManager.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Lets every view be detached into its own window and attached back.
//   An AppObject produced by the ClientAppContext (Context.Windows.Detached).
//
// DETACH
//   - Command "view.detach" (shortcut Ctrl+Shift+D, view header button, tab context menu,
//     or dragging the view's header out of the window).
//   - Creates a DetachedViewWindow, recreates the view in it and moves the SAME ViewModel
//     instance there (XAML elements cannot move between windows/XamlRoots, ViewModels can) -
//     so no state is lost.
//   - The main window navigates back to the previous view; the detached view is listed in the
//     navigation wheel and in the shell's "Windows" menu.
//
// ATTACH
//   - Command "view.attach" (Ctrl+Shift+A, button in the detached window's title bar):
//     the view is recreated in the main window's content frame with the same ViewModel and
//     the detached window closes.
//   - Closing a detached window closes the view (ViewModel.OnClosing, unsaved-changes prompt).
//
// RULES
//   - Any navigation target can be detached unless its descriptor says Detachable = false.
//   - The same view (navigation key + parameter) exists at most once; detaching it again
//     brings the existing window to front.
//   - Hot-swap of a module: its detached windows keep their position; the views are rebuilt
//     with the new module version in the same windows.
//   - Module unloaded or permission removed: the window closes; its saved placement is kept so
//     it returns when the module is back.
//
// SESSION SAVE AND RESTORE
//   - On app close: for every open detached view, store its navigation key, parameter
//     (serializable), module ID and placement in the WindowPlacementStore.
//   - On app start (after modules are loaded, see StartupSequence): reopen every saved detached
//     view at its saved monitor position; views whose module is not available are skipped
//     and kept for the next start.
//
// DOCK LAYOUT
//   - With the dock layout, "detach" means "float": the DockManager calls this manager to create,
//     track and restore FloatingDockWindows. Window creation, monitor-aware placement and the
//     AppData session stay here; what is shown inside the window is decided by the DockManager.
//   - Attach = dock back to the view's last docked position.
