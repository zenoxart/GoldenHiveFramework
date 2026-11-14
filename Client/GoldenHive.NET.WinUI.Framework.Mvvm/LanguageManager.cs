// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : LanguageManager.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Manages the application languages. Equivalent of SprachenManager in WIFI.Anwendung.
//
// CONTENTS
//   - DefaultList: languages supported by the app's resources (code + display name).
//   - Current: the current language; Set(languageCode) falls back to the first default
//     language if the code is unknown.
//   - Refresh(): re-translates the language display names into the current UI language.
//   - Applies the language to WinUI: ApplicationLanguages.PrimaryLanguageOverride (packaged)
//     or the MRT Core ResourceContext, plus CultureInfo.CurrentUICulture.
//   - LanguageChanged event, so modules can reload their resources.
//   - The last used language is stored in the user settings and restored at startup.
//
// TEXT LOOKUP
//   - GetText(TextKey, parameters) / Text(TextKey, parameters) -> string / LocalizedText.
//   - Fallback chain: current language (de-AT) -> parent language (de) -> module default
//     language -> host default language (en) -> "[scope:key]" marker.
//   - Missing keys are logged once per key and listed in the localization report; in DEBUG the
//     marker is shown visibly so gaps are noticed during development.
//   - Sources: the host's resources (.resw/PRI, scope "shell") and each module's texts via its
//     ModuleResourceProvider (scope = module ID); module texts are unloaded with the module.
//   - Pseudo-localization mode (setting): accents and ~40 % longer texts to test layouts in
//     every size class.
//   - Changing the language at runtime updates all LocalizedText instances, command labels,
//     tooltips and menus immediately - no restart.
