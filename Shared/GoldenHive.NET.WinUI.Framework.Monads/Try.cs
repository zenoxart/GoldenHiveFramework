// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Monads
// File    : Try.cs
// Layer   : Shared / Monads
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Boundary between exception-throwing code (framework, database driver, HTTP client) and the
//   monadic world.
//
// USE
//   - Try.Run(() => ...) / Try.RunAsync(...) executes the call and returns Result<T>; known
//     exceptions are mapped to specific kinds (timeout -> Timeout, unique constraint -> Conflict,
//     HTTP 503 -> Unavailable), everything else -> Unexpected (logged with full details).
//   - Used only in adapters (repositories, external clients); domain and use-case code never
//     catch exceptions.
