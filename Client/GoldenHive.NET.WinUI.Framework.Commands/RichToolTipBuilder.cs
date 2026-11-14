// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : RichToolTipBuilder.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Builds the standard tooltip for commands and buttons.
//
// LAYOUT
//   Line 1: label (bold) + shortcut text from the ShortcutManager ("Speichern  Strg+S").
//   Line 2: tooltip / description text.
//   Line 3 (only when disabled): the localized disabled reason, in the secondary colour.
//
// BEHAVIOUR
//   - Same look everywhere (buttons, menus, tabs, wheel segments, tool window headers).
//   - Respects the Windows tooltip delay and Narrator; text is also exposed as HelpText.
//   - Max width ~ 320 epx, wraps; uses ThemeResourceKeys for spacing and typography.
