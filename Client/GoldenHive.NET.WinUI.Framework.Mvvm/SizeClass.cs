// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : SizeClass.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Responsive size classes shared by every view and ViewModel (responsive first).
//
// CLASSES (effective pixels of the view's own available width - NOT the screen width)
//   - Compact  : < 640    one column, navigation collapsed, secondary panels as overlays
//   - Medium   : 640-1007 two columns where useful, navigation as compact pane
//   - Expanded : 1008-1919 full layout, navigation pane open, side panels docked
//   - Wide     : >= 1920  extra columns / side-by-side master-detail
//   Height classes (Short < 500, Regular) for landscape phones-sized windows and docked panes.
//
// WHY "AVAILABLE WIDTH"
//   A view can be shown in the main window, in a narrow side pane or detached in its own small
//   window - it must adapt to the space it actually gets.
//
// CONTRACT
//   - IResponsiveView (implemented by ViewModelAppObject): SizeClass, HeightClass properties,
//     SizeClassChanged event; set by the view host (frame or detached window), never computed
//     by the ViewModel itself.
