// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Monads
// File    : Error.cs
// Layer   : Shared / Monads
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   A failure as a value. Immutable record.
//
// FIELDS
//   - Kind: Validation, NotFound, Conflict, Forbidden, Unauthorized, Unavailable, Timeout,
//           Unexpected.
//   - Code: stable machine-readable code, prefixed with the module/service ("documents.invoice.locked").
//   - Message: TextKey for the user-facing, localized message (client resolves the text).
//   - Details: optional field errors (field -> list of TextKeys) and metadata.
//   - Inner: optional causing Error / exception summary (never sent to clients in production).
//
// COMBINING
//   Several errors can be combined (Error.Many) - used by Validation to report all field errors
//   at once.
