// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleLoader
// File    : ModuleCatalogClient.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Asks the Module Registry which modules the user may use.
//
// BEHAVIOUR
//   - GET via the API Gateway, passing the host version and Windows App SDK version so the
//     server only returns compatible versions.
//   - Response: list of manifests incl. package URL, hash, signature, hotSwap flag.
//   - Cache the last response locally (offline start).
