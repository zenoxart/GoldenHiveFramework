// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.CommandContracts
// File    : ShortcutScope.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Where a shortcut is active. More specific scopes win over general ones.
//
// SCOPES (highest priority first)
//   1. Focus     : a specific control or editor has focus (e.g. a grid, a text editor).
//   2. View      : a module page / workflow step is active (key = navigation key or step type).
//   3. Module    : any page of a module is active (key = module ID).
//   4. Window    : a specific window type (e.g. document window).
//   5. Global    : everywhere in the app.
//   Reserved shell shortcuts sit above all scopes and cannot be overridden by modules.
//
// TEXT INPUT
//   While a text input has focus, gestures without Ctrl/Alt/Win (plain letters, Shift+letter,
//   Delete, Home, ...) are never treated as shortcuts.
