// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Host.Client
// File    : StartupSequence.cs
// Layer   : Client / Host
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Describes the startup sequence from App.OnLaunched to a fully loaded shell.
//   Runs after Program.Main has created the ClientAppContext.
//
// SEQUENCE
//   1. Enforce single instance (AppInstance.FindOrRegisterForKey); a second instance
//      redirects its activation to the first one and exits.
//   2. Determine the activation kind (normal launch, toast, push, protocol) -> ActivationHandler.
//   3. Produce the main ViewModel: Context.Produce<ShellViewModel>().
//   4. ShellViewModel.Start<ShellWindow>(): creates the window, sets the DataContext,
//      restores the window position via Context.Windows and registers the window's
//      DispatcherQueue with Context.Commands.Dispatcher (WIFI: Anwendung.Starten<Hauptfenster>()).
//   5. Sign in via Context.Security (MSAL with the WAM broker).
//   6. Fetch the enabled modules via Context.Modules (Module Registry).
//   7. Load the modules: each gets its own ModuleAppContext and AssemblyLoadContext.
//   8. Connect Context.Notifications to NotificationPoint (SignalR); register WNS and toasts.
//   9. Check for a host update (MSIX / App Installer) - notify only, applied on next start.
//  10. Navigate to the start page.
//
// FAILURE CASES
//   - Server unreachable: load the last known local modules (offline mode) if their
//     signatures are valid; show a clear notice in an InfoBar.
//   - A module fails to load: skip it, log it and report it to NotificationPoint.
//
// WINDOW RESTORE
//   - Step 4: the main window is placed with the WindowPlacementStore (saved monitor and position).
//   - After step 7 (modules loaded): the DetachedViewManager reopens the detached views that
//     were open at the last close, each on its saved monitor position.
//
// DOCK LAYOUT RESTORE
//   - After modules are loaded: DockLayoutStore restores the main layout and floating windows
//     (each on its saved monitor), then the views inside them are recreated.
