// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleContracts
// File    : ViewDescriptor.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   How a module's view takes part in the dock layout (registered with its navigation target).
//
// FIELDS
//   - Role: Document (opens in the document well) or ToolWindow (docks at a side).
//   - DefaultDock: DocumentWell, Left, Right, Top, Bottom, Floating; DefaultAutoHide.
//   - DefaultSize (proportion of the side) and MinimumSize (epx).
//   - AllowMultipleInstances (documents only, e.g. one tab per document ID).
//   - CanClose, CanFloat, CanAutoHide.
//   - Title and icon (resource keys), dirty-state support (unsaved changes marker).
