using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.XamlMetadata;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Path = Microsoft.UI.Xaml.Shapes.Path;
using Windows.Foundation;
using Windows.UI;

namespace GoldenHive.NET.WinUI.Framework.Navigation;

/// <summary>
/// The radial navigation wheel control: inner ring = modules, outer ring = open documents and floating
/// views; the selected segment is drawn in the Golden Hive accent. Pointer direction selects, click confirms.
/// </summary>
public sealed class NavigationWheel : Grid
{
    private const double Size = 460;
    private const double Center = Size / 2;
    private const double InnerFrom = 70;
    private const double InnerTo = 150;
    private const double OuterFrom = 156;
    private const double OuterTo = 222;
    private const double Gap = 1.2;

    private static readonly Color Gold = ColorHelper.FromArgb(255, 245, 184, 42);
    private readonly NavigationWheelViewModel _viewModel;
    private readonly IconCatalog? _icons;
    private readonly Canvas _canvas = new() { Width = Size, Height = Size };
    private readonly TextBlock _title = new() { FontWeight = FontWeights.SemiBold, FontSize = 16, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.Wrap, MaxWidth = 120, MaxLines = 2 };
    private readonly TextBlock _subtitle = new() { FontSize = 12, Opacity = 0.7, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.Wrap, MaxWidth = 120 };
    private readonly TextBlock _hint = new() { FontSize = 12, Opacity = 0.8, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.Wrap, MaxWidth = 440, Margin = new Thickness(0, 8, 0, 0) };

    public NavigationWheel(NavigationWheelViewModel viewModel, IconCatalog? icons)
    {
        _viewModel = viewModel;
        _icons = icons;
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Center;
        RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var centerPanel = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Spacing = 2, IsHitTestVisible = false };
        centerPanel.Children.Add(_title);
        centerPanel.Children.Add(_subtitle);
        Children.Add(_canvas);
        Children.Add(centerPanel);
        Ui.SetText(_hint, "shell:wheel.hint");
        SetRow(_hint, 1);
        Children.Add(_hint);

        AutomationProperties.SetName(this, "Navigation wheel");
        _canvas.PointerMoved += OnPointerMoved;
        _canvas.Tapped += (_, _) => _viewModel.Confirm();
        _viewModel.Changed += (_, _) => Redraw();
        ActualThemeChanged += (_, _) => Redraw();
    }

    private static Point PointAt(double radius, double angle)
    {
        var radians = angle * Math.PI / 180;
        return new Point(Center + (radius * Math.Sin(radians)), Center - (radius * Math.Cos(radians)));
    }

    private static Path CreateSegment(double from, double to, double innerRadius, double outerRadius, Brush fill, Brush stroke)
    {
        var sweep = to - from;
        var figure = new PathFigure { StartPoint = PointAt(outerRadius, from), IsClosed = true, IsFilled = true };
        figure.Segments.Add(new ArcSegment { Point = PointAt(outerRadius, to), Size = new Size(outerRadius, outerRadius), SweepDirection = SweepDirection.Clockwise, IsLargeArc = sweep > 180 });
        figure.Segments.Add(new LineSegment { Point = PointAt(innerRadius, to) });
        figure.Segments.Add(new ArcSegment { Point = PointAt(innerRadius, from), Size = new Size(innerRadius, innerRadius), SweepDirection = SweepDirection.Counterclockwise, IsLargeArc = sweep > 180 });
        var geometry = new PathGeometry();
        geometry.Figures.Add(figure);
        return new Path { Data = geometry, Fill = fill, Stroke = stroke, StrokeThickness = 1 };
    }

    private void Redraw()
    {
        _canvas.Children.Clear();
        var dark = ActualTheme == ElementTheme.Dark;
        var background = new SolidColorBrush(dark ? ColorHelper.FromArgb(242, 32, 32, 32) : ColorHelper.FromArgb(245, 249, 249, 249));
        var segmentFill = new SolidColorBrush(dark ? ColorHelper.FromArgb(255, 45, 45, 45) : Colors.White);
        var stroke = new SolidColorBrush(dark ? ColorHelper.FromArgb(70, 255, 255, 255) : ColorHelper.FromArgb(50, 0, 0, 0));
        var selectedFill = new SolidColorBrush(Gold);
        var text = new SolidColorBrush(dark ? Colors.White : Colors.Black);
        var selectedText = new SolidColorBrush(Colors.Black);

        var hasOuter = _viewModel.Segments.Any(s => s.Ring == 1);
        var radius = (hasOuter ? OuterTo : InnerTo) + 8;
        var disc = new Ellipse { Width = radius * 2, Height = radius * 2, Fill = background, Stroke = stroke, StrokeThickness = 1 };
        Canvas.SetLeft(disc, Center - radius);
        Canvas.SetTop(disc, Center - radius);
        _canvas.Children.Add(disc);

        for (var ring = 0; ring <= 1; ring++)
        {
            var segments = _viewModel.Segments.Where(s => s.Ring == ring).ToList();
            if (segments.Count == 0)
            {
                continue;
            }

            var (from, to) = ring == 0 ? (InnerFrom, InnerTo) : (OuterFrom, OuterTo);
            var sweep = 360.0 / segments.Count;
            for (var i = 0; i < segments.Count; i++)
            {
                var segment = segments[i];
                var isSelected = ReferenceEquals(segment, _viewModel.Selected);
                var start = (i * sweep) - (sweep / 2) + Gap;
                var end = (i * sweep) + (sweep / 2) - Gap;
                if (segments.Count == 1)
                {
                    start = -179.9;
                    end = 179.9;
                }

                _canvas.Children.Add(CreateSegment(start, end, from, to, isSelected ? selectedFill : segmentFill, stroke));
                AddLabel(segment, i * sweep, (from + to) / 2, isSelected ? selectedText : text, ring == 0, segments.Count);
            }
        }

        _title.Text = _viewModel.CenterTitle;
        _subtitle.Text = _viewModel.CenterSubtitle;
    }

    private void AddLabel(WheelSegment segment, double angle, double radius, Brush foreground, bool showText, int count)
    {
        var point = PointAt(radius, angle);
        var panel = new StackPanel { Width = 84, Spacing = 2, IsHitTestVisible = false };
        var icon = _icons?.CreateIcon(segment.IsFloating ? "detach" : segment.Icon) ?? new FontIcon { Glyph = "" };
        icon.Foreground = foreground;
        icon.HorizontalAlignment = HorizontalAlignment.Center;
        if (icon is FontIcon fontIcon)
        {
            fontIcon.FontSize = 18;
        }

        panel.Children.Add(icon);
        if (showText || count <= 8)
        {
            panel.Children.Add(new TextBlock
            {
                Text = segment.Label,
                Foreground = foreground,
                FontSize = 11,
                TextAlignment = TextAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis,
                MaxLines = 1,
            });
        }

        Canvas.SetLeft(panel, point.X - 42);
        Canvas.SetTop(panel, point.Y - 18);
        _canvas.Children.Add(panel);
    }

    private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        var position = e.GetCurrentPoint(_canvas).Position;
        var dx = position.X - Center;
        var dy = position.Y - Center;
        var distance = Math.Sqrt((dx * dx) + (dy * dy));
        if (distance < InnerFrom)
        {
            return;
        }

        var angle = (Math.Atan2(dx, -dy) * 180 / Math.PI + 360) % 360;
        _viewModel.SelectAt(distance > OuterFrom - 3 ? 1 : 0, angle);
    }
}
