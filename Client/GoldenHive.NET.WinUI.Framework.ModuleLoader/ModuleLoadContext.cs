// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.ModuleLoader
// File    : ModuleLoadContext.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   A dedicated, collectible AssemblyLoadContext per module version.
//
// RULES
//   - Shared with the default context (never load again!):
//       Microsoft.WinUI, Microsoft.WindowsAppRuntime.*, Microsoft.Windows.SDK.NET,
//       WinRT.Runtime, GoldenHive.NET.WinUI.Framework.Mvvm, GoldenHive.NET.WinUI.Framework.Monads
//       and all GoldenHive.*.Contracts assemblies.
//     Otherwise types are duplicated and casts to contracts fail.
//   - Module-specific dependencies are resolved from the module folder
//     (AssemblyDependencyResolver based on the module's .deps.json).
//   - Native DLLs via LoadUnmanagedDll from the module folder.
//
// IMPORTANT
//   An AssemblyLoadContext is not a security boundary. Modules run with the full
//   rights of the host process - only load signed, trusted modules.
