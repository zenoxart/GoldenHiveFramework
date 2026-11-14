// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.Core
// File    : RepositoryConventions.cs
// Layer   : Backend / Core
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Rules for data access in the monad architecture.
//
// RULES
//   - Lookups return Task<Option<T>>; lists return Task<IReadOnlyList<T>> (empty, never null).
//   - Writes return Task<Result<Unit>> (or the stored entity); database exceptions are converted
//     with Try (unique constraint -> Conflict, concurrency token -> Conflict, timeout -> Timeout).
//   - Repositories are adapters - no business rules inside.
