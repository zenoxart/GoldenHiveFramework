// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Windowing
// File    : WindowManager.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Manages the application windows and their positions. An AppObject produced by the
//   ClientAppContext (Context.Windows). Equivalent of FensterManager in WIFI.Anwendung.
//
// POSITIONS
//   - Store(position): add or update the position of a window (name, state, left, top,
//     width, height); only take values that are present (null keeps the old value).
//   - Get(windowName): return the stored position or null.
//   - The key is the window name plus the current monitor count (e.g. "ShellWindow1_M2"),
//     so a window is not restored onto a monitor that no longer exists (WIFI 2021-02-04).
//     In WinUI the monitors come from DisplayArea.FindAll(); additionally check that the
//     position lies inside a current DisplayArea's work area.
//   - Save(): write the list via JsonController to LocalDataPath\Windows.json;
//     errors are reported via OnErrorOccurred.
//   - The list is loaded lazily; a missing file (new user) is not an error.
//
// WINDOWS
//   - Apply/read positions through AppWindow (Move, Resize, OverlappedPresenter state).
//   - Main window and optional extra windows (e.g. a document in its own window).
//   - Per window: register its DispatcherQueue with Context.Commands.Dispatcher on open and
//     unregister it on close; the main window's queue becomes the default queue.
//
// NAVIGATION WHEEL
//   Each window has its own navigation wheel and activation history; a window losing
//   activation closes its open wheel.
//
// SHORTCUTS
//   Registers every window's root element with the ShortcutInputRouter on open and removes it on close.
//
// RESPONSIVE AND DETACHED VIEWS
//   - Owns a ResponsiveLayoutManager per window and the DetachedViewManager.
//   - Window positions are now persisted through the WindowPlacementStore with a stable monitor
//     identity (replaces the monitor-count key); the main window and every detached view are
//     restored on the monitor they were on.
