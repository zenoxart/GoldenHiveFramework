// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Docking
// File    : DockLayoutStore.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Persists dock layouts in AppData.
//
// LOCATION
//   LocalDataPath\Layout\DockLayout.json (JsonController), next to WindowLayout.json:
//   - packaged:   %LocalAppData%\Packages\<package family>\LocalState\Layout\
//   - unpackaged: %LocalAppData%\GoldenHive\<Title>\<Version>\Layout\
//
// CONTENTS
//   - The current layout of the main window and of every floating window (DockLayoutModel);
//     floating window positions are referenced from the WindowPlacementStore (monitor-aware).
//   - Named layouts ("Window > Save Window Layout", "Apply Window Layout", up to 10, Ctrl+Alt+n
//     avoided because of AltGr) and "Reset Window Layout" back to the default layout.
//   - Default layout = shell defaults + the modules' ViewDescriptors.
//
// RULES
//   - Saved on every layout change (debounced) and on app close; restored at start before the
//     detached/floating views are reopened.
//   - Views whose module is missing stay as placeholders; unknown future fields are preserved;
//     the model version allows migrations.
//   - Optionally synced via the Configuration Service ("use my layout on other devices"),
//     applied with the local monitor setup.
