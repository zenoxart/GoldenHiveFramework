// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Monads
// File    : _Project.cs
// Layer   : Shared / Monads
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PROJECT
//   Class library net10.0 without any dependencies: the self-written monads of the Golden Hive
//   monad architecture. Used by every backend service and module backend; available to the
//   client as well (e.g. API clients return Result instead of throwing).
//
// WHY SELF-WRITTEN
//   Same reasoning as the self-written MVVM: a small, stable surface the team fully controls
//   (no large functional library in every service and module), named and documented in our terms.
//
// CORE IDEA
//   - Expected outcomes are values, not exceptions: "not found", "invalid", "forbidden",
//     "conflict" are returned as Error inside Result / Option / Validation.
//   - Use cases are railway pipelines: each step runs only if the previous one succeeded;
//     the first failure short-circuits to the end (Bind), success values are transformed (Map).
//   - Exceptions remain only for truly unexpected situations (bugs, infrastructure crashes) and
//     are converted to Error.Unexpected at the infrastructure boundary (Try).
//
// CONTENTS
//   Unit, Error, Option<T>, Result<T>, Validation<T>, Try, AsyncExtensions, LinqExtensions,
//   Combinators (Ensure, Tap, Recover, Traverse/Sequence).
