using System.Text.Json;
using System.Text.Json.Nodes;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Data;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Hosting;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Security;
using GoldenHive.NET.WinUI.Framework.Contracts;
using GoldenHive.NET.WinUI.Framework.Monads;
using Microsoft.Extensions.Options;

namespace GoldenHive.NET.WinUI.Framework.Backend.Core.Pipeline;

/// <summary>Reusable railway steps that use cases compose.</summary>
public static class Steps
{
    /// <summary>ValidateStep: runs the request's validation rules (all errors at once).</summary>
    public static Result<T> Validate<TRequest, T>(TRequest request, Func<TRequest, Validation<T>> rules) =>
        request is null ? Error.Validation("request.missing", "validation:requestMissing") : rules(request).ToResult();

    /// <summary>AuthorizeStep: the (RunAs) user needs the permission.</summary>
    public static Result<Unit> Authorize(GoldenHiveUser user, string permission) =>
        !user.IsAuthenticated
            ? Error.Unauthorized("auth.required", "auth:required")
            : user.Has(permission)
                ? Result.Success()
                : Error.Forbidden("auth.permission", "auth:permissionMissing").WithMetadata("permission", permission);

    public static Result<T> Authorize<T>(this Result<T> result, GoldenHiveUser user, string permission) =>
        result.Bind(value => Authorize(user, permission).Map(_ => value));

    public static async Task<Result<T>> Authorize<T>(this Task<Result<T>> task, GoldenHiveUser user, string permission) =>
        (await task).Authorize(user, permission);

    /// <summary>Requires an authenticated caller and passes the user on.</summary>
    public static Result<GoldenHiveUser> Authenticated(GoldenHiveUser user) =>
        user.IsAuthenticated ? user : Error.Unauthorized("auth.required", "auth:required");

    /// <summary>NotFound error with a consistent code.</summary>
    public static Error NotFound(string entity, object id) =>
        Error.NotFound($"{entity}.notFound", $"{entity.Split('.')[0]}:{entity.Split('.').Last()}.notFound").WithMetadata("id", id);
}

/// <summary>AuditStep: records who did what (user, workflow engine, AI agent) with the outcome.</summary>
public sealed class AuditLog(Db db, IOptions<GoldenHiveOptions> options, TimeProvider time)
{
    public Task<Result<Unit>> WriteAsync(DbSession session, GoldenHiveUser user, string action, string entityType, string? entityId,
        string outcome = "succeeded", JsonObject? details = null, CancellationToken ct = default) =>
        session.ExecuteAsync(Sql, Parameters(user, action, entityType, entityId, outcome, details), ct).ToUnit();

    public Task<Result<Unit>> WriteAsync(GoldenHiveUser user, string action, string entityType, string? entityId,
        string outcome = "succeeded", JsonObject? details = null, CancellationToken ct = default) =>
        db.ExecuteAsync(Sql, Parameters(user, action, entityType, entityId, outcome, details), ct).ToUnit();

    private const string Sql = """
        insert into platform.audit_log (id, at, service, user_id, user_name, actor, action, entity_type, entity_id, outcome, details)
        values (@id, @at, @service, @userId, @userName, @actor, @action, @entityType, @entityId, @outcome, @details::jsonb)
        """;

    private object Parameters(GoldenHiveUser user, string action, string entityType, string? entityId, string outcome, JsonObject? details) => new
    {
        id = Guid.NewGuid(),
        at = time.GetUtcNow(),
        service = options.Value.ServiceName,
        userId = user.Id == Guid.Empty ? (Guid?)null : user.Id,
        userName = user.UserName,
        actor = user.Actor,
        action,
        entityType,
        entityId,
        outcome,
        details = details?.ToJsonString() ?? "{}",
    };
}

/// <summary>IdempotencyStep: returns the stored result for a repeated key (workflow engine, retries).</summary>
public static class Idempotency
{
    public static async Task<Result<T>> RunAsync<T>(DbSession session, string key, Func<Task<Result<T>>> work, CancellationToken ct = default)
    {
        var stored = await session.QuerySingleAsync("select response::text as response from platform.idempotency where key = @key",
            r => r.Get<string>("response"), new { key }, ct);
        if (stored.TryGetValue(out var option) && option.TryGetValue(out var json))
        {
            return Try.Run(() => JsonSerializer.Deserialize<T>(json, GoldenHiveJson.Options)!, "idempotency.read");
        }

        var result = await work();
        if (result.TryGetValue(out var value))
        {
            var saved = await session.ExecuteAsync(
                "insert into platform.idempotency (key, response, created_at) values (@key, @response::jsonb, now()) on conflict (key) do nothing",
                new { key, response = JsonSerializer.Serialize(value, GoldenHiveJson.Options) }, ct);
            if (saved.IsFailure)
            {
                return saved.Error;
            }
        }

        return result;
    }
}
