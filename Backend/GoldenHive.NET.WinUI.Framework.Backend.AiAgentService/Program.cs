using GoldenHive.NET.WinUI.Framework.Backend.AiAgentService;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Data;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Hosting;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Http;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Pipeline;
using GoldenHive.NET.WinUI.Framework.Monads;
using GoldenHive.NET.WinUI.Framework.WorkflowContracts.Api;

var builder = WebApplication.CreateBuilder(args);
builder.AddGoldenHiveDefaults("ai-agent-service");
if (string.Equals(builder.Configuration["GoldenHive:Ai:Provider"], "openai", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddSingleton<IAiProvider, OpenAiCompatibleProvider>();
}
else
{
    builder.Services.AddSingleton<IAiProvider, RulesAiProvider>();
}

builder.Services.AddSingleton<AiAuditLog>();
builder.Services.AddSingleton<AiGuardrails>();
builder.Services.AddSingleton<AiToolCatalog>();
builder.Services.AddSingleton<AiWorkflowAgent>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<AiWorkflowAgent>());

var app = builder.Build();
app.UseGoldenHiveDefaults();

var api = app.MapApi("/api/ai");
api.MapGet("/status", async (IAiProvider provider, AiAuditLog audit, Db db, RequestContext context, CancellationToken ct) =>
    await Steps.Authenticated(context.User)
        .BindAsync(user => audit.TodayAsync(user.TenantId, ct))
        .BindAsync(async today =>
        {
            var enabled = await db.ScalarAsync<bool?>("select enabled from ai.switches where scope = 'global'", null, ct);
            return Result.Success(new AiStatus(provider.Name, provider.Model, enabled.GetValueOrDefault(true) ?? true)
            {
                RunsToday = today.Runs,
                TokensToday = today.Tokens,
            });
        })
        .ToHttp());
api.MapGet("/audit", (Guid? instanceId, AiAuditLog audit, RequestContext context, CancellationToken ct) =>
    Steps.Authorize(context.User, "workflows.view")
        .BindAsync(_ => instanceId is { } id ? audit.ForInstanceAsync(id, ct) : audit.RecentAsync(context.User.TenantId, ct))
        .ToHttp());
api.MapPost("/switch", (AiSwitchRequest request, Db db, AuditLog auditLog, RequestContext context, CancellationToken ct) =>
    Steps.Authorize(context.User, "ai.admin")
        .BindAsync(_ => db.ExecuteAsync(
            """
            insert into ai.switches (scope, enabled, changed_at, changed_by) values ('global', @enabled, now(), @by)
            on conflict (scope) do update set enabled = excluded.enabled, changed_at = now(), changed_by = excluded.changed_by
            """, new { enabled = request.Enabled, by = context.User.UserName }, ct))
        .TapAsync(_ => auditLog.WriteAsync(context.User, request.Enabled ? "ai.enabled" : "ai.disabled", "ai", "global", ct: ct))
        .Map(_ => request)
        .ToHttp());

app.MapInternal().MapPost("/runs", (AiRunRequest request, AiWorkflowAgent agent) =>
{
    agent.Enqueue(request);
    return Results.Accepted();
});

app.Run();
