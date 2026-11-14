// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Navigation
// File    : NavigationWheelViewModel.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   State and logic of the navigation wheel: a radial overlay to switch between modules and
//   navigate inside them, shown while Ctrl+Tab is held. A ViewModelAppObject, one per window.
//
// DYNAMIC CONTENT
//   - Built from the command tree for placement "shell.wheel" (central command manager), so it
//     always reflects the currently loaded modules, the user's permissions and hot-swaps.
//   - Inner ring: one segment per module - the module's primary navigation target
//     ("navigate.<module>.home"), with the module's icon, display name and a badge
//     (e.g. waiting workflow tasks of that module).
//   - Outer ring: when a module segment is highlighted, its second-level entries from the same
//     tree (the module's navigation targets / groups under shell.wheel/<moduleId>).
//   - Order of the inner ring: most recently used first (ModuleActivationHistory), so a
//     quick Ctrl+Tab and release jumps back to the previous module - like Alt+Tab.
//   - Overflow: more than 8 segments per ring -> a "More" segment that opens the next page.
//   - Rebuilt on the command manager's TreeChanged event (module loaded/unloaded/swapped,
//     permissions changed); while the wheel is open, changes are applied after it closes.
//
// STATE
//   IsOpen, Segments (inner + outer), SelectedSegment, SelectedPath (module + target),
//   CenterTitle/CenterSubtitle (shown in the middle of the wheel).
//
// ACTIONS
//   Open(), MoveNext(), MovePrevious(), SelectByDirection(angle), DrillIn(), DrillOut(),
//   Confirm() -> executes the segment's navigation command through the command manager,
//   Cancel().
//
// DETACHED VIEWS
//   - Detached views appear as segments marked with a window badge; confirming one brings
//     its window to front (also from inside a detached window).
//
// DOCK LAYOUT
//   - Besides modules, the wheel offers the open documents and active tool windows (like the
//     Visual Studio IDE navigator on Ctrl+Tab): an extra ring "Open documents" and "Tool windows".
