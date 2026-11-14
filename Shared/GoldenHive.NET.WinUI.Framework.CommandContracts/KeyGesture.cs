// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.CommandContracts
// File    : KeyGesture.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Describes a keyboard shortcut independently of WinUI types.
//
// FORMS
//   - Single     : modifiers + key, e.g. Ctrl+S, Ctrl+Shift+P, F5.
//   - Chord      : two gestures in sequence, e.g. Ctrl+K, Ctrl+D (second within ~1.5 s).
//   - Hold       : a gesture whose action lasts while the modifier is held, e.g. Ctrl+Tab
//                  for the navigation wheel (key-down opens, modifier release confirms).
//
// FIELDS
//   Modifiers (Ctrl, Shift, Alt, Win), virtual key, optional second gesture, kind.
//
// RULES
//   - Stored by virtual key, not by character, so a binding works on every keyboard layout.
//   - Display text is produced for the current layout and language ("Strg+Umschalt+P" on German).
//   - Ctrl+Alt+<key> is avoided as a default, because on many layouts it is AltGr and types
//     a character (e.g. @, {, } on German keyboards).
//   - Serializable as text ("Ctrl+K Ctrl+D") for settings files and import/export.
