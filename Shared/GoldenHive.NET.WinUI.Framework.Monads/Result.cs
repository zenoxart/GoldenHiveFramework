// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Monads
// File    : Result.cs
// Layer   : Shared / Monads
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The central monad: either a success with a value or a failure with an Error.
//
// CASES
//   Success(value) | Failure(error)
//
// OPERATIONS (described)
//   - Map(f)        transform the success value
//   - Bind(f)       continue with a step that itself returns a Result (railway switch)
//   - MapError(f)   transform the error (e.g. add context)
//   - Ensure(pred, error)  turn a success into a failure if a rule is violated
//   - Tap(action)   side effect on success (logging, metrics) without changing the value
//   - Recover(f)    turn certain failures into a success (e.g. NotFound -> default)
//   - Match(onSuccess, onFailure)  leave the railway - only at the edges (HTTP mapping)
//
// RULES
//   - No access to the value without handling the failure case (no .Value that throws).
//   - Implicit conversions from T and Error keep pipelines short.
