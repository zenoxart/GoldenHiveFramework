// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.ModuleDeliveryService
// File    : PackageSigning.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Signatures and checksums.
//
// BEHAVIOUR
//   - Signing happens in CI with a protected key (e.g. HSM / Key Vault),
//     never in the running service.
//   - The service delivers hash and signature together with the manifest.
