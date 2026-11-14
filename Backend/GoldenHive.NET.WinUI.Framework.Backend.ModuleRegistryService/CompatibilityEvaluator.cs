using GoldenHive.NET.WinUI.Framework.Backend.Core.Http;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Pipeline;
using GoldenHive.NET.WinUI.Framework.Contracts.Api;
using GoldenHive.NET.WinUI.Framework.ModuleContracts;
using GoldenHive.NET.WinUI.Framework.ModuleContracts.Api;
using GoldenHive.NET.WinUI.Framework.Monads;

namespace GoldenHive.NET.WinUI.Framework.Backend.ModuleRegistryService;

/// <summary>
/// Builds the catalogue for a user: picks the version assigned by the rollout (pilot / general availability) and
/// evaluates compatibility with the caller's host - with the same rules as the client loader (ModuleCompatibility).
/// </summary>
public sealed class CompatibilityEvaluator(ModuleRegistry registry, ServiceClient services, RequestContext context)
{
    public Task<Result<IReadOnlyList<ModuleCatalogEntry>>> CatalogAsync(string? hostVersion, string? windowsAppSdk, CancellationToken ct) =>
        Steps.Authorize(context.User, "modules.view")
            .BindAsync(_ => registry.PublishedAsync(ct))
            .BindAsync(async published =>
            {
                var assignments = (await services.GetAsync<IReadOnlyList<ModuleAssignment>>(
                        "update-orchestrator", $"/internal/assignments?userId={context.User.Id}", null, ct))
                    .GetValueOrDefault([]);
                var host = new HostInfo(hostVersion ?? "1.0.0", windowsAppSdk ?? "1.8.0");
                var chosen = published.GroupBy(m => m.Manifest.ModuleId, StringComparer.OrdinalIgnoreCase)
                    .Select(g =>
                    {
                        var assignment = assignments.FirstOrDefault(a => a.ModuleId.Equals(g.Key, StringComparison.OrdinalIgnoreCase));
                        var match = assignment is null ? null : g.FirstOrDefault(m => m.Manifest.Version == assignment.Version);
                        return (Module: match ?? g.First(), Stage: match is null ? null : assignment!.Stage.ToString());
                    })
                    .ToList();
                var manifests = chosen.Select(c => c.Module.Manifest).ToList();
                return Result.Success<IReadOnlyList<ModuleCatalogEntry>>(chosen.Select(c =>
                {
                    var check = ModuleCompatibility.Check(c.Module.Manifest, host, manifests);
                    return new ModuleCatalogEntry(c.Module.Manifest, c.Module.Channel, c.Module.PublishedAt)
                    {
                        Compatible = check.IsValid,
                        Problems = check.Errors.Select(e => e.Message).ToList(),
                        PackageUrl = c.Module.Manifest.Client?.Package is null ? null : $"/api/packages/{c.Module.Manifest.ModuleId}/{c.Module.Manifest.Version}",
                        RolloutStage = c.Stage,
                    };
                }).ToList());
            });
}
