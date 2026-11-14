// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : Ui.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Attached properties that make icons, texts and tooltips one-liners in XAML - in the host's
//   compiled XAML and in the modules' loose XAML alike. Namespace prefix: gh
// (xmlns:gh="using:GoldenHive.NET.WinUI.Framework.Commands").
//
// PROPERTIES
//   gh:Ui.Command="documents.create"
//       Binds the control to the command from the central command manager and fills in
//       everything else: icon, label, localized tooltip (with shortcut and disabled reason),
//       AutomationProperties.Name / HelpText, keyboard accelerator text.
//       Works on Button, AppBarButton, MenuFlyoutItem, SplitButton, ToggleButton, HyperlinkButton.
//   gh:Ui.Icon="save"
//       Sets the icon of any button-like control (Icon / Content with icon + text).
//   gh:Ui.Text="shell:common.cancel"
//       Localized Content / Label / Text / Header depending on the control.
//   gh:Ui.ToolTip="documents:invoice.tooltip.approve"
//       Localized tooltip; gh:Ui.ToolTipDescription for a second, longer line.
//   gh:Ui.IconPosition="Left|Top|Only"  and  gh:Ui.ShowLabel="Auto|True|False"
//
// EXAMPLE (described)
//   An AppBarButton with only gh:Ui.Command="documents.approve" shows the "approve" icon, the
//   label "Freigeben"/"Approve", and the tooltip "Freigeben (Strg+Umschalt+F)" with
//   "Keine Berechtigung" when disabled - in the current language.
//
// BEHAVIOUR
//   - All values update live on language, theme, shortcut and CanExecute changes (weak events).
//   - Explicitly set properties (e.g. a custom tooltip) win over the command's defaults.
//   - A markup extension {gh:Loc key} is offered for compiled XAML; loose XAML uses the
//     attached properties, which work through the aggregated XAML metadata provider.
