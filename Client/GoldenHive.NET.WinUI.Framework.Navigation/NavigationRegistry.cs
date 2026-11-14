// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Navigation
// File    : NavigationRegistry.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Directory of all navigation targets.
//
// ENTRY
//   Key (e.g. "documents.list"), module ID, ViewModel factory, view factory,
//   required permission.
//
// INTEGRATION WITH THE COMMAND MANAGER
//   - Every navigation target is also registered as a command "navigate.<key>" with a
//     placement in "shell.navigation" (and optionally in the main menu).
//   - Titles, icons, grouping and order therefore come from the command descriptor;
//     the navigation tree is a command tree like any other menu.
//
// BEHAVIOUR
//   - Modules add targets on initialization and remove them on unload.
//
// NAVIGATION WHEEL
//   - A module's primary navigation target is automatically placed in "shell.wheel" as the
//     module's inner-ring segment (navigate.<module>.home).
//   - Further targets appear in the module's outer ring when they declare a placement under
//     "shell.wheel/<moduleId>".
//
// DETACHABLE
//   - Each target declares Detachable (default true), MinimumSize for its detached window and
//     whether its parameter can be serialized for session restore.
