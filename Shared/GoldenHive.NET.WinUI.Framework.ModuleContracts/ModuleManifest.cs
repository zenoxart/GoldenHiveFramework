// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleContracts
// File    : ModuleManifest.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Data model of the module manifest (see Modules/*/manifest.jsonc).
//
// FIELDS
//   moduleId, version, minHostVersion,
//   client: platform (winui3), targetFramework, windowsAppSdk (version range),
//           package, sha256, signature, entryAssembly, entryType,
//           viewMode (looseXaml | codeOnly | compiledXaml),
//           hotSwap (supported | restartRequired),
//   backend: service, apiVersion, route,
//   permissions: list of permission names,
//   dependencies: list of moduleId + versionRange.
//
// WORKFLOW FIELDS
//   contractSurface: reference to the generated ModuleContractSurface of this version
//                    (commands, input schemas, step types, events, templates, migrations).
//
// PRESENTATION FIELDS
//   displayName (resource key), icon (glyph or image in the package) and accentColor (optional),
//   used by the navigation wheel, the navigation tree and the impact overview.
//
// LOCALIZATION FIELDS
//   languages: languages the module ships texts for (e.g. de, en); defaultLanguage.
//   The CI check fails if a key is missing in one of the declared languages.
