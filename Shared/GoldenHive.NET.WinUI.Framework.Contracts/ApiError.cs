// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Contracts
// File    : ApiError.cs
// Layer   : Shared / Contracts
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Uniform error format between backend and client.
//
// CONTENTS
//   - Based on RFC 9457 (Problem Details): type, title, status, detail, correlation ID.
//   - Error codes per module, prefixed with the module ID.
//
// MONAD ARCHITECTURE
//   - ApiError is the wire format of Monads.Error: Kind -> HTTP status + type URI, Code, TextKey of
//     the message, field errors. The client's API clients turn it back into Error, so client code
//     can use Result as well.
