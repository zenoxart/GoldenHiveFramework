// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Monads
// File    : Combinators.cs
// Layer   : Shared / Monads
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Helpers for collections and parallel work.
//
// CONTENTS
//   - Traverse / Sequence: list of Results -> Result of list (first failure wins) and the
//     Validation variant (all failures collected).
//   - Combine: several independent Results -> tuple; WhenAll for async results.
//   - FirstSuccess: try alternatives in order (e.g. cache, then database).
