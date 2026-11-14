// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Windowing
// File    : WindowPlacementStore.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Stores window placements and the detached-view session in AppData.
//
// LOCATION
//   LocalDataPath\Layout\WindowLayout.json (JsonController)
//   - packaged:   %LocalAppData%\Packages\<package family>\LocalState\Layout\
//   - unpackaged: %LocalAppData%\GoldenHive\<Title>\<Version>\Layout\
//   Local (not roaming) on purpose: monitor setups are specific to each machine.
//
// CONTENT PER WINDOW
//   - Window key: "main" or "detached:<navigationKey>:<parameterHash>".
//   - Monitor identity: stable monitor ID (device interface ID from
//     Windows.Devices.Display.DisplayMonitor), plus monitor resolution and work area as fallback
//     match. DisplayArea IDs are not used for storage because they change between sessions.
//   - Placement relative to that monitor's work area (left, top, width, height in physical
//     pixels) and the monitor's DPI scale at the time of saving.
//   - State: normal, maximized, minimized-is-stored-as-normal; always-on-top flag.
//   - For detached views: module ID, navigation key, serialized parameter, open at close (bool).
//   - Saved on every move/resize (debounced) and on app close, so a crash does not lose layout.
//
// RESTORE RULES
//   1. Saved monitor connected -> place the window there at the saved relative position,
//      scaled if the DPI changed.
//   2. Monitor missing -> use the monitor with the same resolution/arrangement if found,
//      otherwise the primary monitor (the window is never restored off-screen).
//   3. Always clamp the window into the target work area (taskbar respected) and to the
//      window type's minimum size.
//   4. Maximized windows are first placed on the saved monitor, then maximized there.
