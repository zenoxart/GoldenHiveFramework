using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Graphics;

namespace GoldenHive.NET.WinUI.Framework.Windowing;

/// <summary>
/// Manages the application windows (WIFI: FensterManager): restores and saves their placement per monitor
/// in AppData, enforces minimum sizes, registers each window's DispatcherQueue with the command pool's
/// dispatcher and connects its keyboard input to the shortcut manager.
/// </summary>
public sealed class WindowManager : AppObject
{
    private readonly Dictionary<string, Window> _windows = new(StringComparer.OrdinalIgnoreCase);
    private WindowPlacementStore? _placements;
    private ResponsiveLayoutManager? _responsive;

    public event EventHandler<Window>? WindowClosed;

    public WindowPlacementStore Placements => _placements ??= AppContext.Produce<WindowPlacementStore>();

    public ResponsiveLayoutManager Responsive => _responsive ??= AppContext.Produce<ResponsiveLayoutManager>();

    public IReadOnlyDictionary<string, Window> Windows => _windows;

    /// <summary>
    /// Starts managing a window. Call after its Content is set and before Activate():
    /// restores the saved placement, applies the minimum size and wires up dispatcher and shortcuts.
    /// </summary>
    public void Track(Window window, string key, int minWidth = 360, int minHeight = 480, bool isMain = false)
    {
        _windows[key] = window;
        var appWindow = window.AppWindow;
        if (appWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.PreferredMinimumWidth = minWidth;
            presenter.PreferredMinimumHeight = minHeight;
        }

        var maximize = Placements.Get(key) is { } placement && Restore(appWindow, placement);

        var commands = (AppContext as ICommandContext)?.Commands;
        commands?.Dispatcher.Register(key, window.DispatcherQueue, isMain);
        if (window.Content is UIElement root)
        {
            commands?.InputRouter.Attach(root);
        }

        var activatedOnce = false;
        window.Activated += (_, _) =>
        {
            if (activatedOnce)
            {
                return;
            }

            activatedOnce = true;
            if (maximize && appWindow.Presenter is OverlappedPresenter overlapped)
            {
                overlapped.Maximize();
            }
        };

        appWindow.Changed += (_, e) =>
        {
            if (e.DidPositionChange || e.DidSizeChange || e.DidPresenterChange)
            {
                Capture(appWindow, key);
            }
        };

        window.Closed += (_, _) =>
        {
            _windows.Remove(key);
            commands?.Dispatcher.Unregister(key);
            if (window.Content is UIElement content)
            {
                commands?.InputRouter.Detach(content);
            }

            WindowClosed?.Invoke(this, window);
        };

        AppContext.Log.Write($"Window '{key}' is managed", LogEntryType.Information, nameof(WindowManager));
    }

    private static DisplayArea? FindDisplay(WindowPlacement placement)
    {
        var all = DisplayArea.FindAll();
        for (var i = 0; i < all.Count; i++)
        {
            var bounds = all[i].OuterBounds;
            if (bounds.X == placement.MonitorX && bounds.Y == placement.MonitorY && bounds.Width == placement.MonitorWidth && bounds.Height == placement.MonitorHeight)
            {
                return all[i];
            }
        }

        // Same resolution elsewhere (monitors rearranged) is the next best match.
        for (var i = 0; i < all.Count; i++)
        {
            var bounds = all[i].OuterBounds;
            if (bounds.Width == placement.MonitorWidth && bounds.Height == placement.MonitorHeight)
            {
                return all[i];
            }
        }

        return null;
    }

    private bool Restore(AppWindow appWindow, WindowPlacement placement)
    {
        try
        {
            if (placement.Width <= 0 || placement.Height <= 0)
            {
                return placement.Maximized;
            }

            var display = FindDisplay(placement) ?? DisplayArea.Primary;
            var work = display.WorkArea;
            var width = Math.Min(placement.Width, work.Width);
            var height = Math.Min(placement.Height, work.Height);
            var x = work.X + Math.Clamp(placement.X, 0, Math.Max(0, work.Width - width));
            var y = work.Y + Math.Clamp(placement.Y, 0, Math.Max(0, work.Height - height));
            appWindow.MoveAndResize(new RectInt32(x, y, width, height));
            return placement.Maximized;
        }
        catch (Exception ex)
        {
            OnErrorOccurred(ex);
            return false;
        }
    }

    private void Capture(AppWindow appWindow, string key)
    {
        try
        {
            if (!appWindow.IsVisible)
            {
                return;
            }

            var state = (appWindow.Presenter as OverlappedPresenter)?.State ?? OverlappedPresenterState.Restored;
            if (state == OverlappedPresenterState.Minimized)
            {
                return;
            }

            var display = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Nearest);
            var placement = Placements.Get(key) ?? new WindowPlacement { Key = key };
            placement.Maximized = state == OverlappedPresenterState.Maximized;
            placement.MonitorX = display.OuterBounds.X;
            placement.MonitorY = display.OuterBounds.Y;
            placement.MonitorWidth = display.OuterBounds.Width;
            placement.MonitorHeight = display.OuterBounds.Height;
            if (!placement.Maximized)
            {
                placement.X = appWindow.Position.X - display.WorkArea.X;
                placement.Y = appWindow.Position.Y - display.WorkArea.Y;
                placement.Width = appWindow.Size.Width;
                placement.Height = appWindow.Size.Height;
            }

            Placements.Set(placement);
        }
        catch (Exception ex)
        {
            AppContext.Log.Write($"Could not capture the placement of '{key}': {ex.Message}", LogEntryType.Warning, nameof(WindowManager));
        }
    }
}
