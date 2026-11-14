// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Docking
// File    : ToolWindowPane.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Hosts tool windows (views with role ToolWindow) docked at a side or floating.
//
// TYPICAL TOOL WINDOWS
//   Module Explorer (navigation tree, left), Properties of the selection (right),
//   Output / log (bottom), Notifications (bottom), Workflow inbox and AI approvals (bottom/right),
//   plus tool windows contributed by modules.
//
// BEHAVIOUR
//   - Pin button toggles auto-hide; Shift+Esc hides; Alt+- opens the pane menu (dock, float, auto-hide).
//   - Remembers its size per side.
