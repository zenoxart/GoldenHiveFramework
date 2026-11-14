// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleLoader
// File    : ModulePackageDownloader.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Downloads module packages over HTTPS from the Module Delivery Service.
//
// BEHAVIOUR
//   - Download into a temporary file, report progress to the NotificationClient.
//   - Resume / retry with backoff on interruption.
//   - Move into the ModuleStore only after successful validation.
