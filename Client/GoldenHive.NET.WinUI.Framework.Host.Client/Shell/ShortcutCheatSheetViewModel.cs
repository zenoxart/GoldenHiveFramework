// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Host.Client
// File    : ShortcutCheatSheetViewModel.cs
// Layer   : Client / Host
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Overlay that shows the shortcuts available right now. A ViewModelAppObject.
//
// BEHAVIOUR
//   - Opens while Ctrl is held for about one second (or via Ctrl+/), closes on release / Esc.
//   - Shows only bindings of the active scopes, grouped: this view, this module, global.
//   - Entries come from the ShortcutManager and update while the overlay is open.
