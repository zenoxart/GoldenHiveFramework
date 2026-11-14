// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Host.Client
// File    : Program.cs
// Layer   : Client / Host
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Custom entry point of the WinUI app, so the application context exists before the
//   WinUI Application starts. Equivalent of the static Main in WIFI.CS.Teil2's App.
//
// PROJECT SETTING
//   Define DISABLE_XAML_GENERATED_MAIN in the host .csproj so the XAML compiler does not
//   generate its own Main.
//
// SEQUENCE IN MAIN ([STAThread])
//   1. XamlCheckProcessRequirements / WinRT initialization (as the generated Main does).
//   2. Create the ClientAppContext (the root context).
//   3. Write "<title> starts" to the log.
//   4. Restore the last language from the settings (fallback: OS UI language, two-letter code)
//      via Languages.Set(...); Languages.Refresh().
//   5. Application.Start(...): inside the callback create a DispatcherQueueSynchronizationContext
//      and the App, passing it the context (App.Context property).
//   6. Everything else continues in App.OnLaunched (see StartupSequence).
//   7. After Application.Start returns: save settings, language and window positions,
//      shut the context down.
//
// WINDOW SESSION
//   Before shutdown: Context.Windows saves the placement of the main window and of every open
//   detached view (WindowPlacementStore in AppData).
