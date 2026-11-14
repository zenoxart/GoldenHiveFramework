// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Windowing
// File    : ResponsiveLayoutManager.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Makes the whole app responsive first. One instance per window.
//
// BEHAVIOUR
//   - Observes the size of each layout host (window content, content frame, side panes,
//     detached view windows) and computes its SizeClass / HeightClass with hysteresis
//     (a few pixels) so layouts do not flicker at a breakpoint while resizing.
//   - Pushes the classes to the hosted views (visual state) and their ViewModels
//     (IResponsiveView), on the command pool's dispatcher.
//   - Reacts to DPI changes (moving a window to another monitor), text scaling
//     (Settings > Accessibility > Text size) and orientation changes.
//
// RULES FOR ALL VIEWS (shell, modules, workflow forms, dialogs)
//   - No fixed widths/heights for content; use Grid star sizing, MinWidth/MaxWidth,
//     wrapping panels, ScrollViewer for overflow - every view must work from 320 to 3840 epx.
//   - Layout switches through VisualStateManager states named after the size classes
//     (Compact, Medium, Expanded, Wide); this works in loose XAML of modules as well.
//   - Readable line length: text content max ~ 900 epx wide, centred in Wide.
//   - Touch-friendly targets (min 40x40 epx) in Compact.
//   - Resizing never loses state (scroll position, selection, entered values).
//
// WINDOW SIZE LIMITS
//   - Minimum window size per window type via OverlappedPresenter (PreferredMinimumWidth /
//     Height), e.g. main window 360 x 480 epx, detached views 320 x 240 epx.
