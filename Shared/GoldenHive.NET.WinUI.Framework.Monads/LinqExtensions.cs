// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Monads
// File    : LinqExtensions.cs
// Layer   : Shared / Monads
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Select / SelectMany / Where for Option, Result and Task<Result>, so multi-step pipelines that
//   need several earlier values can use C# query syntax (from ... in ... select ...) instead of
//   nested lambdas.
//
// GUIDELINE
//   Fluent Bind/Map chains for linear pipelines; query syntax when later steps need values of
//   several earlier steps.
