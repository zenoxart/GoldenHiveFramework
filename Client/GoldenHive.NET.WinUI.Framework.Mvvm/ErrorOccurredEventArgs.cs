// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : ErrorOccurredEventArgs.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Event data and handler delegate for AppObject.ErrorOccurred.
//   Equivalent of FehlerAufgetretenEventArgs / FehlerAufgetretenEventHandler.
//
// CONTENTS
//   - Cause: the exception that describes the error (read-only, set in the constructor).
//   - Optional: Handled flag so a handler can mark the error as dealt with,
//     and Severity (Information, Warning, Error) for the log and InfoBar.
