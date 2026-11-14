// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleContracts
// File    : ModuleCompatibility.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Rules for whether a module fits a host.
//
// CHECKS
//   - Host version >= minHostVersion.
//   - The host's Windows App SDK runtime is within the windowsAppSdk range.
//   - Contract versions compatible (same major version).
//   - Evaluated identically on the server (Registry) and the client (Loader).
