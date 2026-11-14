# =============================================================================
# Project : GoldenHive.NET.WinUI.Framework.Backend.AiAgentService
# File    : Dockerfile
# Layer   : Backend / Docker
# Status  : Architecture sketch - comments only, no implementation yet
# =============================================================================
#
# PURPOSE
#   Container image for AiAgentService.
#
# STRUCTURE (multi-stage)
#   1. Build stage with the .NET 10 SDK image: restore, publish (Release).
#   2. Runtime stage with the ASP.NET 10 runtime image (chiseled/minimal), non-root user.
#   3. Port via environment variable, HEALTHCHECK on /health/live.
#   Image tag = service version; built in the CI pipeline.
