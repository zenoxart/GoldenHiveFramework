// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Monads
// File    : AsyncExtensions.cs
// Layer   : Shared / Monads
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Async versions of all operations on Task<Result<T>> and Task<Option<T>>, so async pipelines
//   read like sync ones: MapAsync, BindAsync, EnsureAsync, TapAsync, MatchAsync.
//
// RULES
//   - CancellationToken is passed through every step.
//   - ConfigureAwait is not needed in ASP.NET Core; library code still avoids sync-over-async.
