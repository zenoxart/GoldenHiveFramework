// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : CommandDispatcher.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   The UI dispatcher of the application. Lives inside the RelayCommandPool and is
//   exposed as RelayCommandPool.Dispatcher; implements IUiDispatcher (Mvvm).
//   Wraps Microsoft.UI.Dispatching.DispatcherQueue.
//
// WHY IN THE COMMAND POOL
//   Commands are the main bridge between background work and the UI: CanExecuteChanged,
//   checked state, TreeChanged and execution results must all reach the UI thread of the
//   right window. Owning the dispatcher in the pool keeps that routing in one place.
//
// RESPONSIBILITIES
//   - Window queues: Register(windowId, DispatcherQueue) / Unregister(windowId),
//     called by the WindowManager when a window opens/closes. The main window's queue
//     is the default queue.
//   - HasThreadAccess, Run(action), RunAsync(func) on the default queue via TryEnqueue.
//   - Run(windowId, action) for window-specific work (e.g. requery for one window's commands).
//   - Priority: High for state changes (CanExecute, checked), Normal for everything else.
//   - Coalescing: several requeries for the same command within one UI frame are merged
//     into one CanExecuteChanged.
//   - Shutdown: after the last window closes, reject new work and complete pending tasks.
//
// USERS
//   - RelayCommandPool / PooledCommand: CanExecuteChanged, checked state, TreeChanged.
//   - CommandExecutor: completion, progress and error notifications.
//   - ViewModelAppObject and LogManager through ApplicationContext.Dispatcher (IUiDispatcher),
//     which the ClientAppContext points at this instance.
//   - NotificationClient: incoming SignalR events.
