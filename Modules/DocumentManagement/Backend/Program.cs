using System.Text.Json.Nodes;
using GoldenHive.NET.Modules.DocumentManagement.Backend;
using GoldenHive.NET.Modules.DocumentManagement.Backend.UseCases;
using GoldenHive.NET.Modules.DocumentManagement.Contracts;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Hosting;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Http;
using GoldenHive.NET.WinUI.Framework.Backend.Core.Pipeline;
using GoldenHive.NET.WinUI.Framework.Contracts.Api;
using GoldenHive.NET.WinUI.Framework.Monads;

var builder = WebApplication.CreateBuilder(args);
builder.AddGoldenHiveDefaults("document-service");
builder.Services.AddSingleton<DocumentRepository>();
builder.Services.AddScoped<CreateDocument>();
builder.Services.AddScoped<UpdateDocument>();
builder.Services.AddScoped<AddDocumentVersion>();
builder.Services.AddScoped<ArchiveDocument>();
builder.Services.AddScoped<CreateInvoice>();
builder.Services.AddScoped<SubmitInvoice>();
builder.Services.AddScoped<DecideInvoice>();
builder.Services.AddScoped<PayInvoice>();
builder.Services.AddScoped<ArchiveInvoice>();
builder.Services.AddScoped<DispatchInvoice>();
builder.Services.AddScoped<WorkflowSteps>();

var app = builder.Build();
app.UseGoldenHiveDefaults();

var api = app.MapApi("/api/documents");

// ---- documents
api.MapGet("/", (string? search, string? category, string? status, int? skip, int? take, DocumentRepository repository, RequestContext context, CancellationToken ct) =>
    Steps.Authorize(context.User, DocumentPermissions.View)
        .BindAsync(_ => repository.SearchAsync(context.User.TenantId, search, category, status, Math.Max(0, skip ?? 0), Math.Clamp(take ?? 50, 1, 200), ct))
        .ToHttp());
api.MapGet("/stats", (DocumentRepository repository, RequestContext context, CancellationToken ct) =>
    Steps.Authorize(context.User, DocumentPermissions.View).BindAsync(_ => repository.StatsAsync(context.User.TenantId, ct)).ToHttp());
api.MapGet("/{id:guid}", (Guid id, DocumentRepository repository, RequestContext context, CancellationToken ct) =>
    Steps.Authorize(context.User, DocumentPermissions.View).BindAsync(_ => repository.FindAsync(id, ct)).ToHttp());
api.MapGet("/{id:guid}/versions", (Guid id, DocumentRepository repository, RequestContext context, CancellationToken ct) =>
    Steps.Authorize(context.User, DocumentPermissions.View).BindAsync(_ => repository.VersionsAsync(id, ct)).ToHttp());
api.MapPost("/", (CreateDocumentRequest request, CreateDocument useCase, CancellationToken ct) =>
    useCase.ExecuteAsync(request, ct).ToCreated(d => $"/api/documents/{d.Id}"));
api.MapPut("/{id:guid}", (Guid id, UpdateDocumentRequest request, UpdateDocument useCase, CancellationToken ct) => useCase.ExecuteAsync(id, request, ct).ToHttp());
api.MapPost("/{id:guid}/versions", (Guid id, AddVersionRequest request, AddDocumentVersion useCase, CancellationToken ct) => useCase.ExecuteAsync(id, request, ct).ToHttp());
api.MapPost("/{id:guid}/archive", (Guid id, ArchiveDocument useCase, CancellationToken ct) => useCase.ExecuteAsync(id, ct).ToHttp());

// ---- invoices
api.MapGet("/invoices", (string? status, string? search, DocumentRepository repository, RequestContext context, CancellationToken ct) =>
    Steps.Authorize(context.User, DocumentPermissions.View).BindAsync(_ => repository.InvoicesAsync(context.User.TenantId, status, search, ct)).ToHttp());
api.MapGet("/invoices/{id:guid}", (Guid id, DocumentRepository repository, RequestContext context, CancellationToken ct) =>
    Steps.Authorize(context.User, DocumentPermissions.View).BindAsync(_ => repository.FindInvoiceAsync(id, ct)).ToHttp());
api.MapPost("/invoices", (CreateInvoiceRequest request, CreateInvoice useCase, CancellationToken ct) =>
    useCase.ExecuteAsync(request, ct).ToCreated(i => $"/api/documents/invoices/{i.Id}"));
api.MapPost("/invoices/{id:guid}/submit", (Guid id, SubmitInvoice useCase, CancellationToken ct) => useCase.ExecuteAsync(id, ct).ToHttp());
api.MapPost("/invoices/{id:guid}/approve", (Guid id, InvoiceDecisionRequest request, DecideInvoice useCase, CancellationToken ct) =>
    useCase.ApproveAsync(id, request.Comment, ct).ToHttp());
api.MapPost("/invoices/{id:guid}/reject", (Guid id, InvoiceDecisionRequest request, DecideInvoice useCase, CancellationToken ct) =>
    useCase.RejectAsync(id, request.Comment, ct).ToHttp());
api.MapPost("/invoices/{id:guid}/pay", (Guid id, PayInvoice useCase, CancellationToken ct) => useCase.ExecuteAsync(id, ct).ToHttp());

// ---- lookups (EntityRef fields of workflow forms)
api.MapGet("/lookups/cost-centers", (DocumentRepository repository, CancellationToken ct) =>
    repository.CostCentersAsync(ct).Map(list => list.Select(r => new LookupItem(r.Value, r.Label))).ToHttp());

// ---- headless workflow steps (same use cases; caller = RunAs user, delegated for the engine / AI)
api.MapPost("/workflow-steps/{stepType}", (string stepType, JsonObject inputs, WorkflowSteps steps, CancellationToken ct) =>
    steps.ExecuteAsync(stepType, inputs, ct).ToHttp());

app.Run();
