// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Monads
// File    : Validation.cs
// Layer   : Shared / Monads
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Applicative validation: runs ALL checks and collects every error, instead of stopping at the
//   first one like Result.
//
// USE
//   - Validating incoming requests and smart constructors of value objects
//     (e.g. EmailAddress.Create, Amount.Create).
//   - Combine independent validations (Apply / Combine) into one value object or command.
//   - ToResult(): continues on the Result railway with Error.Kind = Validation and all field
//     errors in Details (shown next to the fields in the client form / workflow step).
