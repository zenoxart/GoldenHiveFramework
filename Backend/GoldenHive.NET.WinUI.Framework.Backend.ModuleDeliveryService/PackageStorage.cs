// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.ModuleDeliveryService
// File    : PackageStorage.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Storage of module packages.
//
// BEHAVIOUR
//   - Blob/object storage; packages are immutable (version = key).
//   - Served with range requests (resumable download), optionally via a CDN.
//   - Access only for users the module is enabled for.
