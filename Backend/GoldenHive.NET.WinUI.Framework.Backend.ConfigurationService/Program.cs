using System.Text.Json;
using GoldenHive.NET.WinUI.Framework.Backend.ConfigurationService;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Hosting;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Http;
using GoldenHive.NET.WinUI.Framework.Contracts.Api;

var builder = WebApplication.CreateBuilder(args);
builder.AddGoldenHiveDefaults("configuration-service");
builder.Services.AddScoped<SettingsStore>();

var app = builder.Build();
app.UseGoldenHiveDefaults();

var api = app.MapApi("/api/config/settings");
api.MapGet("/", (SettingsStore store, CancellationToken ct) => store.ListAsync(ct).ToHttp());
api.MapGet("/{key}", (string key, SettingsStore store, CancellationToken ct) => store.GetAsync(key, ct).ToHttp());
api.MapPut("/{scope}/{key}", (string scope, string key, ConfigSettingUpdate update, SettingsStore store, CancellationToken ct) =>
    store.SetAsync(scope, key, update.Value, ct).ToHttp());
api.MapDelete("/{scope}/{key}", (string scope, string key, SettingsStore store, CancellationToken ct) => store.DeleteAsync(scope, key, ct).ToHttp());

// Services read effective settings for a tenant (and optionally a user).
app.MapInternal().MapGet("/settings/{key}", (string key, Guid tenant, Guid? user, SettingsStore store, CancellationToken ct) =>
    store.Effective(tenant, user, key, ct).ToHttp());

app.Run();
