// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Windowing
// File    : DetachedViewWindow.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The window that hosts one detached view.
//
// CONTENTS
//   - Own TitleBar with the view's title, module icon, and buttons: attach back, always on top
//     (optional), close.
//   - Content host with its own ResponsiveLayoutManager, ShortcutInputRouter registration and
//     navigation wheel (Ctrl+Tab works here too and can bring other windows to front).
//   - Registered with the WindowManager like any other window (DispatcherQueue registration,
//     placement tracking, minimum size).
