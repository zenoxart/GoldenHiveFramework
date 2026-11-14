// GoldenHive.NET.Modules.DocumentManagement.Client
//
// Client part of the document-management module (codeOnly: views are built in code, no XAML metadata needed,
// so the module can be hot-swapped). Loaded by the host's ModuleLoader into its own AssemblyLoadContext;
// framework assemblies are referenced with Private=false and shared with the host.
//
//   DocumentManagementModule  entry point: texts, icons, views, commands, menu group, event subscriptions
//   ViewModels/               Result-based ViewModels calling /api/documents through the module's IModuleApi
//   Views/                    code-built UserControls (UiKit, DataList, InputForm)
// The wire contracts are linked from ..\Contracts (shared with the backend).
