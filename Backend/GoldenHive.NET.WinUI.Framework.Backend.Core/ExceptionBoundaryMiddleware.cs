// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.Core
// File    : ExceptionBoundaryMiddleware.cs
// Layer   : Backend / Core
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Last line of defence: catches exceptions that escaped an adapter (bugs), logs them with the
//   correlation ID and returns a 500 Problem Details. Should practically never fire; every hit is
//   treated as a bug (alert in monitoring).
