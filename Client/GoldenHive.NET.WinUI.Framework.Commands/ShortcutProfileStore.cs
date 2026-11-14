// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : ShortcutProfileStore.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Persists shortcut presets and user overrides.
//
// BEHAVIOUR
//   - User overrides in RoamingDataPath\Shortcuts.json (JsonController) and synchronized with
//     the Configuration Service, so they follow the user to other devices.
//   - Built-in presets shipped with the host; tenant presets from the Configuration Service.
//   - Stores gestures as text ("Ctrl+K Ctrl+D") plus the scope and the command ID.
