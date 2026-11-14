// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Navigation
// File    : NavigationWheel.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The radial navigation wheel control (templated control, built in code - no compiled XAML
//   needed in modules).
//
// LAYOUT
//   - Two concentric rings of segments drawn as arc paths; the segment angle depends on the
//     number of entries (evenly divided, starting at the top).
//   - Centre disc: selected module/target name, subtitle (e.g. "3 open workflow tasks") and
//     the module icon in large.
//   - Each segment: icon, short label, optional badge; outer ring appears next to the
//     highlighted inner segment only (fan-out), not as a full circle.
//   - Shown in a Popup over the shell's content frame, centred on the window (or on the mouse
//     position when opened by mouse), with an acrylic backdrop dimming the page behind it.
//
// LOOK AND MOTION
//   - Golden Hive styling from the host's ThemeResourceKeys: honeycomb-gold accent for the
//     selected segment, light/dark/high-contrast aware.
//   - Composition animations: scale-in on open, highlight follows selection, fan-out of the
//     outer ring; disabled when the user turned off animations in Windows.
//
// ACCESSIBILITY
//   - UI Automation: the wheel is a list, segments are list items with name, position and
//     badge text; Narrator announces the selection while cycling with Tab.
//   - All functions reachable by keyboard alone.
