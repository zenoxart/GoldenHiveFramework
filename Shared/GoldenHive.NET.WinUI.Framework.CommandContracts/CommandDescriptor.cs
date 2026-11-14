// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.CommandContracts
// File    : CommandDescriptor.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Metadata of a command, as registered with the central command manager.
//
// FIELDS
//   - CommandId, label resource key, description, icon, keyboard shortcut.
//   - Kind: normal, toggle (checked state), radio (exclusive within a section).
//   - Required permission and visibility policy (hide or disable when not permitted).
//   - Whether a parameter is expected.
//   - Placements: list of CommandPlacement entries - where this command appears in which
//     menus/trees. One command can appear in several places (main menu, context menu,
//     navigation tree) with a different position in each.
//
// WORKFLOW FIELDS
//   - ParameterSchema: ID + version of the command's parameter schema (InputSchema).
//   - WorkflowCapable: may be used as a Command step in workflows and is recorded by the
//     WorkflowRecorder.
//   - AiAllowed: may be offered to the AI agent as a tool (only if WorkflowCapable and
//     executable headless through the module backend).
//
// SHORTCUT FIELDS
//   - DefaultShortcuts: list of KeyGesture + ShortcutScope suggested by the module.
//     They are defaults only - the ShortcutManager decides the effective binding
//     (tenant and user overrides, conflicts, reserved gestures).
//
// ICON AND TEXT FIELDS
//   - Icon: IconRef (e.g. "save", "documents.invoice").
//   - TextKey: base TextKey; label, tooltip, description and disabled reasons are derived by
//     convention (<key>.label, <key>.tooltip, ...), so a command needs a single key.
//   - ShowLabel: icon only, label only or both (default per placement: toolbar = icon,
//     menu = icon + label).
