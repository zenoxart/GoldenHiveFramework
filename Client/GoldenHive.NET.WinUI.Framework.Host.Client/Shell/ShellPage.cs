// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Host.Client
// File    : ShellPage.cs
// Layer   : Client / Host
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Describes the shell UI. The existing MainPage becomes the ShellPage.
//
// LAYOUT
//   - Custom title bar via AppWindow (ExtendsContentIntoTitleBar) with a search box.
//   - NavigationView on the left; items from ShellViewModel; footer: settings, account.
//   - Content frame for module pages (controlled by NavigationService).
//   - InfoBar stack above the frame for in-app notifications.
//   - Optional CommandBar whose items are contributed by the active module.
//
// BINDING
//   Shell-owned elements use compiled x:Bind bindings to the ShellViewModel.
//
// NAVIGATION WHEEL
//   - Overlay layer above the content frame that hosts the NavigationWheel popup.
//   - The root element routes PreviewKeyDown/PreviewKeyUp to the NavigationWheelInputHandler.
//
// SHORTCUTS
//   The window root's key events are handled by the ShortcutInputRouter (registered via the
//   WindowManager); the cheat sheet overlay shares the overlay layer with the navigation wheel.
//
// RESPONSIVE SHELL
//   - NavigationView PaneDisplayMode switches with the size class: Compact -> LeftMinimal
//     (hamburger), Medium -> LeftCompact (icons), Expanded/Wide -> Left (open pane).
//   - MenuBar collapses into an overflow menu in Compact; the title bar search becomes an icon.
//   - Command bar items move to the secondary (overflow) menu as space shrinks.
//   - InfoBars stack full width in Compact, max 900 epx otherwise.
//   - Every view header has a "detach" button (hidden in Compact, available via menu).
//
// DOCK LAYOUT
//   - The content area is a DockHost instead of a single Frame: title bar, menu bar, toolbar,
//     dock layout (document well + tool windows + auto-hide strips) and status bar.
//   - The NavigationView becomes the "Module Explorer" tool window (left, tree from
//     placement "shell.navigation"); in Compact it is an auto-hide flyout.
//   - Window menu (placement "shell.window"): float, dock, auto-hide, new tab groups,
//     save/apply/reset window layout, list of open windows.
