// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Contracts
// File    : TextKey.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Refers to a localized text by key, so no visible text is hard-coded anywhere.
//
// FORMAT
//   "<scope>:<key>", e.g. "shell:command.save.label", "documents:invoice.tooltip.approve".
//   Scope = "shell" or a module ID; the key is dot-separated.
//
// CONVENTIONS PER COMMAND / BUTTON
//   <key>.label        short text on the button or menu item
//   <key>.tooltip      one-line explanation shown on hover
//   <key>.description  longer help text (rich tooltip, command palette, accessibility HelpText)
//   <key>.disabled.<reason>  why the action is not available right now
//
// PARAMETERS
//   Optional named placeholders ("Delete {count} documents?"), formatted with the current
//   culture (numbers, dates) by the LanguageManager.
