using GoldenHive.NET.WinUI.Framework.Commands;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using VirtualKey = Windows.System.VirtualKey;

namespace GoldenHive.NET.WinUI.Framework.Navigation;

/// <summary>
/// Keyboard handling of the navigation wheel with Alt+Tab semantics: Ctrl+Tab opens it (shown after a short
/// delay), Tab/Shift+Tab and arrows move, releasing Ctrl or Enter confirms, Esc cancels. A quick tap
/// switches straight to the previous module without showing the wheel.
/// </summary>
public sealed class NavigationWheelInputHandler
{
    private readonly NavigationWheelViewModel _viewModel;
    private readonly Action<bool> _setVisible;
    private readonly DispatcherQueueTimer _showTimer;
    private bool _keyboardSession;

    public NavigationWheelInputHandler(NavigationWheelViewModel viewModel, UIElement root, DispatcherQueue queue, Action<bool> setVisible)
    {
        _viewModel = viewModel;
        _setVisible = setVisible;
        _showTimer = queue.CreateTimer();
        _showTimer.Interval = TimeSpan.FromMilliseconds(150);
        _showTimer.IsRepeating = false;
        _showTimer.Tick += (_, _) =>
        {
            _showTimer.Stop();
            if (_viewModel.IsOpen)
            {
                _setVisible(true);
            }
        };

        root.PreviewKeyDown += OnPreviewKeyDown;
        root.PreviewKeyUp += OnPreviewKeyUp;
    }

    /// <summary>Starts a wheel session (from the Ctrl+Tab shortcut or a menu click).</summary>
    public void Begin(bool reverse)
    {
        if (_viewModel.IsOpen)
        {
            _viewModel.Move(reverse ? -1 : +1);
            return;
        }

        _viewModel.Prepare(reverse);
        _keyboardSession = ShortcutInputRouter.IsDown(VirtualKey.Control);
        if (_keyboardSession)
        {
            _showTimer.Start();
        }
        else
        {
            _setVisible(true);
        }
    }

    public void Confirm()
    {
        _showTimer.Stop();
        _setVisible(false);
        _viewModel.Confirm();
    }

    public void Cancel()
    {
        _showTimer.Stop();
        _setVisible(false);
        _viewModel.Cancel();
    }

    private void OnPreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (!_viewModel.IsOpen)
        {
            return;
        }

        switch (e.Key)
        {
            case VirtualKey.Tab:
                _viewModel.Move(ShortcutInputRouter.IsDown(VirtualKey.Shift) ? -1 : +1);
                break;
            case VirtualKey.Right:
                _viewModel.Move(+1);
                break;
            case VirtualKey.Left:
                _viewModel.Move(-1);
                break;
            case VirtualKey.Up:
            case VirtualKey.Down:
                _viewModel.SwitchRing();
                break;
            case VirtualKey.Enter:
                Confirm();
                break;
            case VirtualKey.Escape:
                Cancel();
                break;
            default:
                if (!ShortcutInputRouter.IsModifierKey(e.Key))
                {
                    break;
                }

                return;
        }

        e.Handled = true;
    }

    private void OnPreviewKeyUp(object sender, KeyRoutedEventArgs e)
    {
        if (!_viewModel.IsOpen || !_keyboardSession)
        {
            return;
        }

        if (e.Key is VirtualKey.Control or VirtualKey.LeftControl or VirtualKey.RightControl && !ShortcutInputRouter.IsDown(VirtualKey.Control))
        {
            e.Handled = true;
            Confirm();
        }
    }
}
