// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.ApiGateway
// File    : RoutingConfiguration.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Routing from the gateway to the services and Docker modules.
//
// BEHAVIOUR
//   - Reverse proxy (e.g. YARP); routes come from the Module Registry (backend.route).
//   - TLS termination, rate limiting, token validation, correlation ID propagation.
//   - API versions in the path (/api/v2/...), so older clients keep working.
