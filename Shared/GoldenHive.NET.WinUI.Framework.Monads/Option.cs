// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Monads
// File    : Option.cs
// Layer   : Shared / Monads
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   A value that may be absent - replaces null for "maybe" results (e.g. repository lookups).
//
// CASES
//   Some(value) | None
//
// OPERATIONS (described)
//   Map, Bind, Match, Filter, GetValueOrDefault(fallback), ToResult(error) - turns "absent"
//   into an Error (typically NotFound) to continue on the Result railway.
//
// RULE
//   Backend methods never return null for domain values; they return Option<T>.
