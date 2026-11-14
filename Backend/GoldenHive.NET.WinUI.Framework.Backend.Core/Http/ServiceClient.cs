using System.Net.Http.Headers;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Hosting;
using GoldenHive.NET.WinUI.Framework.Contracts;
using GoldenHive.NET.WinUI.Framework.Monads;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace GoldenHive.NET.WinUI.Framework.Backend.Core.Http;

/// <summary>
/// Service-to-service HTTP calls that return Results. Internal calls carry the internal key; calls on behalf of a
/// user carry a bearer token. The correlation ID of the current request is forwarded.
/// </summary>
public sealed class ServiceClient(IHttpClientFactory factory, IOptions<GoldenHiveOptions> options, IHttpContextAccessor accessor)
{
    public const string InternalKeyHeader = "X-Internal-Key";

    private readonly GoldenHiveOptions _options = options.Value;

    public Task<Result<T>> GetAsync<T>(string service, string path, string? bearer = null, CancellationToken ct = default) =>
        SendAsync<T>(HttpMethod.Get, service, path, null, bearer, ct);

    public Task<Result<T>> PostAsync<T>(string service, string path, object? body, string? bearer = null, CancellationToken ct = default) =>
        SendAsync<T>(HttpMethod.Post, service, path, body, bearer, ct);

    public Task<Result<T>> PutAsync<T>(string service, string path, object? body, string? bearer = null, CancellationToken ct = default) =>
        SendAsync<T>(HttpMethod.Put, service, path, body, bearer, ct);

    public async Task<Result<T>> SendAsync<T>(HttpMethod method, string service, string path, object? body, string? bearer, CancellationToken ct)
    {
        if (ResolveUrl(service, path) is not { } url)
        {
            return Error.Unavailable("service.unknown", $"Service '{service}' is not configured (GoldenHive__Services__{service}).");
        }

        var request = HttpResults.WithBody(method, url, body);
        if (bearer is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
        }
        else
        {
            request.Headers.Add(InternalKeyHeader, _options.InternalKey);
        }

        if (accessor.HttpContext?.TraceIdentifier is { } correlation)
        {
            request.Headers.Add(CorrelationIdMiddleware.Header, correlation);
        }

        return await HttpResults.SendAsync<T>(factory.CreateClient("goldenhive"), request, ct);
    }

    /// <summary>Absolute URL for a service path; the service may also be an absolute URL.</summary>
    public string? ResolveUrl(string service, string path)
    {
        var baseUrl = service.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? service.TrimEnd('/') : _options.ServiceUrl(service);
        return baseUrl is null ? null : $"{baseUrl}/{path.TrimStart('/')}";
    }
}
