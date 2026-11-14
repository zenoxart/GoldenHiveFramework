using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

namespace GoldenHive.NET.WinUI.Framework.Docking;

/// <summary>A resize handle between dock areas (vertical bar resizes columns, horizontal bar rows).</summary>
internal sealed class DockSplitter : Grid
{
    private readonly bool _vertical;
    private readonly Func<double> _getSize;
    private readonly Action<double> _setSize;
    private readonly int _sign;
    private readonly double _min;
    private readonly double _max;
    private Point _start;
    private double _startSize;
    private bool _dragging;

    public DockSplitter(bool vertical, Func<double> getSize, Action<double> setSize, int sign, double min, double max)
    {
        _vertical = vertical;
        _getSize = getSize;
        _setSize = setSize;
        _sign = sign;
        _min = min;
        _max = max;
        Background = new SolidColorBrush(Colors.Transparent);
        ProtectedCursor = InputSystemCursor.Create(vertical ? InputSystemCursorShape.SizeWestEast : InputSystemCursorShape.SizeNorthSouth);
        PointerPressed += OnPointerPressed;
        PointerMoved += OnPointerMoved;
        PointerReleased += OnPointerReleased;
        PointerCaptureLost += (_, _) => EndDrag();
    }

    public event EventHandler? DragCompleted;

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        _start = e.GetCurrentPoint(null).Position;
        _startSize = _getSize();
        _dragging = CapturePointer(e.Pointer);
        e.Handled = true;
    }

    private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_dragging)
        {
            return;
        }

        var position = e.GetCurrentPoint(null).Position;
        var delta = _vertical ? position.X - _start.X : position.Y - _start.Y;
        _setSize(Math.Clamp(_startSize + (delta * _sign), _min, _max));
        e.Handled = true;
    }

    private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        ReleasePointerCapture(e.Pointer);
        EndDrag();
        e.Handled = true;
    }

    private void EndDrag()
    {
        if (_dragging)
        {
            _dragging = false;
            DragCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
