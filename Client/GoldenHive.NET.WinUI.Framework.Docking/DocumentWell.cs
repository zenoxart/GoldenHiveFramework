// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Docking
// File    : DocumentWell.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The document area in the centre: one or more tab groups with the open documents.
//
// BEHAVIOUR
//   - New documents open in the active group; "New Horizontal/Vertical Tab Group" splits.
//   - Ctrl+F6 / Ctrl+Shift+F6 next/previous document, Ctrl+F4 close, Ctrl+Alt+PageUp avoided
//     (AltGr); Ctrl+- / Ctrl+Shift+- navigate backward/forward across documents.
//   - Empty state when no document is open: start page with recent views and workflows.
//   - Unsaved-changes markers (*) on tabs; closing asks via the ViewModel.
