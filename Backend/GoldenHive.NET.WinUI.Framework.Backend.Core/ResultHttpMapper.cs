// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.Core
// File    : ResultHttpMapper.cs
// Layer   : Backend / Core
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Leaves the railway at the HTTP edge: maps Result<T> to an HTTP response (Minimal APIs IResult).
//
// MAPPING
//   Success(value)          -> 200 OK (201 Created with location for creations, 204 for Unit)
//   Validation              -> 400 with field errors
//   Unauthorized            -> 401
//   Forbidden               -> 403
//   NotFound                -> 404
//   Conflict                -> 409
//   Unavailable / Timeout   -> 503 / 504 with Retry-After where sensible
//   Unexpected              -> 500 without internal details (correlation ID only)
//   Errors are written as RFC 9457 Problem Details (ApiError) including Code and the TextKey,
//   so the client shows a localized message.
//
// RULE
//   Endpoints contain no logic: bind the request, call the use case, map the Result.
