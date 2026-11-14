// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Docking
// File    : DockManager.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Central service of the dock layout. An AppObject produced by the ClientAppContext
//   (Context.Dock); one layout per window (main window + floating windows).
//
// RESPONSIBILITIES
//   - Open views as documents or tool windows according to their ViewDescriptor (role,
//     default dock position, size, auto-hide). Opening an already open view activates it.
//   - Documents: open in the active tab group of the document well; multiple instances only if
//     the view allows it (e.g. two different documents of the same module).
//   - Tool windows: one instance each; show, hide (Shift+Esc), pin/unpin (auto-hide), float.
//   - Layout operations: dock to side, split tab group horizontally/vertically, move tab to the
//     next/new tab group, float, re-dock, maximise the document well (Alt+Shift+Enter = full
//     screen with documents only), close all but this, close all documents.
//   - Floating = detached: tearing a tab or tool window off creates a FloatingDockWindow through
//     the Windowing DetachedViewManager; the same ViewModel instance moves with it.
//   - Active document / active tool window tracking (focus, title, status bar, command scopes
//     for the ShortcutManager).
//   - Window menu and "View > Other Windows" are command trees (placements "shell.window" and
//     "shell.toolwindows") built from the registered views.
//   - Raises LayoutChanged; the DockLayoutStore saves it (debounced).
//
// HOT-SWAP / MODULES
//   - Tabs of a swapped module stay where they are; their content is rebuilt with the new
//     module version and the saved ViewModel state.
//   - Module unloaded or permission removed: its tabs close, their positions are kept in the
//     layout as placeholders so they return to the same place later.
