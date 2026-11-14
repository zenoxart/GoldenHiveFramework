// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Security
// File    : TokenStore.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Secure storage of tokens and secrets.
//
// BEHAVIOUR
//   - Protect the MSAL token cache with DPAPI or the Windows Credential Locker.
//   - Never store tokens in plain text in settings or logs.
