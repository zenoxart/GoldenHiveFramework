// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Contracts
// File    : IconRef.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Refers to an icon by a symbolic name instead of a code point or a file path, so icons
//   can be used everywhere (commands, buttons, tabs, wheel, menus) with one short string.
//
// FORMAT
//   - "save", "detach", "workflow"            -> shell icon catalogue (Golden Hive set)
//   - "documents.invoice"                     -> icon registered by a module (prefix = module)
//   - "glyph:E74E"                            -> direct Segoe Fluent Icons glyph (escape hatch)
//   - "image:Assets/invoice.svg"              -> image file inside the module package
//
// RULES
//   - Names are stable and lowercase; renaming an icon keeps the old name as an alias.
//   - No sizes or colours in the reference - the control decides size, the theme decides colour.
