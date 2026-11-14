// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.HotSwap
// File    : UnloadMonitor.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Watches whether an old ModuleLoadContext is actually unloaded.
//
// BEHAVIOUR
//   - Call Unload(), keep the context only through a WeakReference.
//   - Trigger GC several times and check whether the WeakReference is released.
//   - If the context stays alive (typically WinRT/XAML objects holding references),
//     log it and track the memory used.
//   - Above a threshold: show "restart recommended" in the shell.
