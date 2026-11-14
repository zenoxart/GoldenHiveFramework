using System.Collections.Concurrent;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using Microsoft.UI.Dispatching;

namespace GoldenHive.NET.WinUI.Framework.Commands;

/// <summary>
/// The UI dispatcher of the application. Lives inside the RelayCommandPool (RelayCommandPool.Dispatcher)
/// and is exposed to the Mvvm layer as <see cref="IUiDispatcher"/>. Keeps one DispatcherQueue per window;
/// the main window's queue is the default.
/// </summary>
public sealed class CommandDispatcher : IUiDispatcher
{
    private readonly ConcurrentDictionary<string, DispatcherQueue> _queues = new(StringComparer.OrdinalIgnoreCase);
    private DispatcherQueue? _default;

    /// <summary>The default (main window) queue, or null before the first window registered.</summary>
    public DispatcherQueue? Default => _default;

    public bool HasThreadAccess => _default?.HasThreadAccess ?? true;

    /// <summary>Registers the queue of a window (called by the WindowManager).</summary>
    public void Register(string windowKey, DispatcherQueue queue, bool isDefault = false)
    {
        _queues[windowKey] = queue;
        if (isDefault || _default is null)
        {
            _default = queue;
        }
    }

    public void Unregister(string windowKey) => _queues.TryRemove(windowKey, out _);

    /// <summary>Runs on the UI thread; directly if already there.</summary>
    public void Run(Action action)
    {
        var queue = _default;
        if (queue is null || queue.HasThreadAccess)
        {
            action();
            return;
        }

        queue.TryEnqueue(() => action());
    }

    /// <summary>Always enqueues (used to coalesce several changes into one UI update).</summary>
    public void Post(Action action, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        var queue = _default;
        if (queue is null)
        {
            action();
            return;
        }

        queue.TryEnqueue(priority, () => action());
    }

    public Task RunAsync(Action action)
    {
        var queue = _default;
        if (queue is null || queue.HasThreadAccess)
        {
            action();
            return Task.CompletedTask;
        }

        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var enqueued = queue.TryEnqueue(() =>
        {
            try
            {
                action();
                completion.SetResult();
            }
            catch (Exception ex)
            {
                completion.SetException(ex);
            }
        });

        if (!enqueued)
        {
            completion.SetCanceled();
        }

        return completion.Task;
    }
}
