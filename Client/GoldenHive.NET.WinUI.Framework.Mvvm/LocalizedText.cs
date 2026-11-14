// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : LocalizedText.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   A bindable, live-updating localized text. Use it in ViewModels instead of plain strings.
//
// BEHAVIOUR
//   - Created with a TextKey (+ optional parameters) through the context:
//     AppContext.Languages.Text("documents:invoice.title", new { number }).
//   - Value: the text in the current language; raises PropertyChanged when the language changes
//     or a parameter changes - bound UI updates without reloading the view.
//   - Holds the LanguageManager subscription weakly, so it never keeps a view alive.
