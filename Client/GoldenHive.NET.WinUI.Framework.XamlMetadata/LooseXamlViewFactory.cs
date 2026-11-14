// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.XamlMetadata
// File    : LooseXamlViewFactory.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Creates module views from loose XAML.
//
// VARIANTS (manifest field viewMode)
//   - looseXaml : XAML as an embedded resource in the module, loaded with XamlReader.Load;
//                 bindings with {Binding} only, no x:Bind, no code-behind.
//   - codeOnly  : the module builds the view itself in C#; the factory just calls its method.
//   - compiledXaml : only for modules shipped together with the host.
//
// BEHAVIOUR
//   - Set the DataContext to the ViewModel.
//   - Views may use the resource keys from ThemeResourceKeys.
//
// RESPONSIVE
//   - Loose XAML views of modules use VisualStateManager states Compact / Medium / Expanded /
//     Wide (AdaptiveTrigger-free; the ResponsiveLayoutManager sets the state), so module views
//     follow the same breakpoints as the shell.
//   - Views are created per host, so the same view can be recreated in a detached window.
