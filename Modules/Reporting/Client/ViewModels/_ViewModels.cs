// =============================================================================
// Project : GoldenHive.NET.Modules.Reporting.Client
// File    : _ViewModels.cs
// Layer   : Module Reporting
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The module's ViewModels: report catalogue, parameter dialog, result view.
//
// RULES
//   - Derive from ViewModelAppObject and are produced by the ModuleAppContext (Produce<T>),
//     never with "new"; ViewModels that must survive a hot-swap implement IStatefulViewModel.
//   - Use BeginWork/EndWork for long operations (IsBusy + log entries).
//   - Actions only via commands from the RelayCommandPool, not via ICommand fields of their own.
//   - Data only via the module's API client; no direct connections to other modules.
