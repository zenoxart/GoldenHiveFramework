using GoldenHive.NET.WinUI.Framework.Backend.Core.Security;
using Microsoft.AspNetCore.Http;

namespace GoldenHive.NET.WinUI.Framework.Backend.Core.Pipeline;

/// <summary>
/// Who calls and when: the authenticated user (or the RunAs user of a workflow / AI run, marked by Actor),
/// the correlation ID and the current time. Domain code never reads DateTime.Now; time comes from here.
/// </summary>
public sealed class RequestContext(IHttpContextAccessor accessor, TimeProvider time)
{
    private GoldenHiveUser? _user;

    public GoldenHiveUser User => _user ??= accessor.HttpContext is { } http ? GoldenHiveUser.FromPrincipal(http.User) : GoldenHiveUser.Anonymous;

    public string CorrelationId => accessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("n");

    public DateTimeOffset Now => time.GetUtcNow();

    /// <summary>The raw bearer token of the caller (to call other services on the user's behalf).</summary>
    public string? BearerToken
    {
        get
        {
            var header = accessor.HttpContext?.Request.Headers.Authorization.ToString();
            return header is not null && header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? header[7..].Trim() : null;
        }
    }

    /// <summary>Overrides the user for background work (workflow engine, AI agent).</summary>
    public RequestContext As(GoldenHiveUser user)
    {
        _user = user;
        return this;
    }
}
