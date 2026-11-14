using GoldenHive.NET.WinUI.Framework.Backend.Core.Hosting;
using GoldenHive.NET.WinUI.Framework.Contracts;
using GoldenHive.NET.WinUI.Framework.Monads;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GoldenHive.NET.WinUI.Framework.Backend.Core.Http;

/// <summary>
/// Leaves the railway at the HTTP edge: maps Result to an HTTP response.
/// Success -> 200 (201 with location, 204 for Unit); errors -> RFC 9457 Problem Details (ApiError) with Code and TextKey.
/// Endpoints contain no logic: bind the request, call the use case, map the Result.
/// </summary>
public static class ResultHttpMapper
{
    public static IResult ToHttp<T>(this Result<T> result) =>
        result.Match(value => value is Unit ? Results.NoContent() : Results.Json(value, GoldenHiveJson.Options), Problem);

    public static async Task<IResult> ToHttp<T>(this Task<Result<T>> task) => (await task).ToHttp();

    public static IResult ToCreated<T>(this Result<T> result, Func<T, string> location) =>
        result.Match(value => (IResult)new CreatedResult(location(value), Results.Json(value, GoldenHiveJson.Options, statusCode: 201)), Problem);

    public static async Task<IResult> ToCreated<T>(this Task<Result<T>> task, Func<T, string> location) => (await task).ToCreated(location);

    public static IResult Problem(Error error) => new ProblemResult(error);

    /// <summary>Writes the ApiError with the correlation ID; internal details only when ShowErrorDetails is on.</summary>
    private sealed class ProblemResult(Error error) : IResult
    {
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            var options = httpContext.RequestServices.GetService<IOptions<GoldenHiveOptions>>()?.Value;
            var apiError = ApiError.FromError(error, httpContext.TraceIdentifier, options?.ShowErrorDetails == true) with
            {
                Instance = httpContext.Request.Path,
            };
            if (error.Kind is ErrorKind.Unavailable or ErrorKind.Timeout)
            {
                httpContext.Response.Headers.RetryAfter = "5";
            }

            await Results.Json(apiError, GoldenHiveJson.Options, "application/problem+json", apiError.Status).ExecuteAsync(httpContext);
        }
    }

    private sealed class CreatedResult(string location, IResult inner) : IResult
    {
        public Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.Headers.Location = location;
            return inner.ExecuteAsync(httpContext);
        }
    }
}
