// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Host.Client
// File    : ShellViewModel.cs
// Layer   : Client / Host
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Main ViewModel of the shell. Derives from ViewModelAppObject with the typed
//   ClientAppContext. Equivalent of the main ViewModel "Anwendung" in WIFI.CS.Teil2.
//
// START
//   Start<TWindow>() where TWindow : Window, new()
//     - remember the window type for additional windows,
//     - create the window, set the root element's DataContext to this ViewModel,
//     - InitializeWindow(window): unique window name (type name + free number),
//       restore position/size/state via Context.Windows, save it again on Closed,
//       release the DataContext on close so the GC can collect the window,
//     - activate the window.
//
// CONTENTS
//   - Navigation tree: command tree for placement "shell.navigation" from Context.Commands,
//     projected onto the NavigationView.
//   - Main menu: command tree for placement "shell.mainmenu", projected onto a MenuBar.
//   - Both refresh on the command manager's TreeChanged event.
//   - Command palette: search over Context.Commands.Find(text).
//   - In-app notifications (InfoBar area), log view bound to Context.Log.Entries.
//   - User info, tenant, connection status, available updates, IsBusy.
//
// WORKFLOWS
//   - Navigation entries (via command placements): Workflow inbox, Designer, Impact overview,
//     AI approvals - with badges for waiting items and Broken workflows.
//   - InfoBar when a module update affected the user's workflows, linking to the overview.
//
// NAVIGATION WHEEL
//   - Produces one NavigationWheelViewModel per window (Context.Produce<T>()).
//   - Badges per module come from the WorkflowManager (waiting tasks) and notifications.
//
// SHORTCUTS
//   Commands "shell.shortcuts.settings" (Ctrl+K, Ctrl+S) and "shell.shortcuts.cheatsheet"
//   open the ShortcutSettingsViewModel page and the cheat sheet overlay.
//
// DOCK LAYOUT
//   - Built-in tool windows: Module Explorer, Output (bound to Context.Log), Notifications,
//     Workflow inbox, AI approvals, Properties; built-in documents: start page, workflow designer,
//     impact overview, shortcut settings.
