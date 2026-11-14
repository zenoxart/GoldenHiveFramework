// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : IUiDispatcher.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Minimal abstraction of the UI dispatcher for the Mvvm layer.
//
// WHY AN INTERFACE HERE
//   The implementation (CommandDispatcher) lives inside the RelayCommandPool in the Commands
//   project, which builds on Mvvm. Mvvm cannot reference Commands, so it only knows this
//   contract; the ClientAppContext connects the two at startup.
//
// MEMBERS (described)
//   - HasThreadAccess
//   - Run(action) / RunAsync(func): execute on the UI thread (directly if already there).
