// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Host.Client
// File    : ShortcutSettingsViewModel.cs
// Layer   : Client / Host
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Settings page to view and change all shortcuts. A ViewModelAppObject.
//   Opened via Ctrl+K, Ctrl+S, the settings menu or the command palette.
//
// CONTENTS
//   - Table of all commands with their effective shortcuts: command name, module, scope,
//     gesture, source (shell, module, preset, tenant, user).
//   - Search by command name or by pressing a key combination ("which command uses Ctrl+D?").
//   - Filter by module, scope, "changed by me", "conflicts", "orphaned".
//   - Edit: click a row and press the new key combination (recording mode, supports chords);
//     live conflict warning with the option to replace the other binding.
//   - Reset per command, per module, or all; choose a preset; import / export JSON.
//   - Bindings locked by the tenant are shown read-only with the reason.
