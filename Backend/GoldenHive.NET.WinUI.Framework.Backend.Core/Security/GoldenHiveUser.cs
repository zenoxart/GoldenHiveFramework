using System.Security.Claims;

namespace GoldenHive.NET.WinUI.Framework.Backend.Core.Security;

/// <summary>Claim types of Golden Hive tokens.</summary>
public static class GoldenHiveClaims
{
    public const string Subject = "sub";
    public const string Name = "name";
    public const string DisplayName = "display_name";
    public const string Tenant = "tenant";
    public const string Permission = "perm";

    /// <summary>Delegation: the actor that acts for the user (workflow-engine, ai-agent).</summary>
    public const string Actor = "act";

    public const string Service = "svc";
}

/// <summary>The authenticated caller (or the RunAs user of a workflow / AI run).</summary>
public sealed record GoldenHiveUser(Guid Id, string UserName, string DisplayName, Guid TenantId, IReadOnlySet<string> Permissions)
{
    public static GoldenHiveUser Anonymous { get; } = new(Guid.Empty, "anonymous", "Anonymous", Guid.Empty, new HashSet<string>());

    /// <summary>Delegating actor (workflow-engine, ai-agent) or null for a direct user call.</summary>
    public string? Actor { get; init; }

    /// <summary>Name of the calling service for internal service tokens.</summary>
    public string? Service { get; init; }

    public bool IsAuthenticated => Id != Guid.Empty || Service is not null;

    public bool Has(string permission) => Permissions.Contains("*") || Permissions.Contains(permission);

    /// <summary>"maria.huber" or "maria.huber via ai-agent" (for audit entries and history).</summary>
    public string AuditName => Actor is null ? UserName : $"{UserName} via {Actor}";

    public static GoldenHiveUser FromPrincipal(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated != true)
        {
            return Anonymous;
        }

        Guid.TryParse(principal.FindFirstValue(GoldenHiveClaims.Subject), out var id);
        Guid.TryParse(principal.FindFirstValue(GoldenHiveClaims.Tenant), out var tenant);
        var name = principal.FindFirstValue(GoldenHiveClaims.Name) ?? "unknown";
        return new GoldenHiveUser(
            id,
            name,
            principal.FindFirstValue(GoldenHiveClaims.DisplayName) ?? name,
            tenant,
            principal.FindAll(GoldenHiveClaims.Permission).Select(c => c.Value).ToHashSet(StringComparer.OrdinalIgnoreCase))
        {
            Actor = principal.FindFirstValue(GoldenHiveClaims.Actor),
            Service = principal.FindFirstValue(GoldenHiveClaims.Service),
        };
    }
}
