// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleLoader
// File    : ModuleStore.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Local storage of module packages.
//
// LOCATION
//   - Packaged (MSIX): ApplicationData.LocalFolder\Modules\<moduleId>\<version>\
//   - Unpackaged:      %LocalAppData%\GoldenHive\Modules\<moduleId>\<version>\
//
// RULES
//   - Versions live side by side; the active version is recorded in an index file.
//   - Delete old versions only on the next start (files may still be loaded).
//   - Limit the number of versions kept for rollback.
