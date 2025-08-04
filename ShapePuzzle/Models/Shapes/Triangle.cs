using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Media;

namespace ShapePuzzle.Models.Shapes;

public sealed class Triangle : Shape
{
    public Triangle(ShapeType type, Point position) : base(type, position)
    {
        Points = new ObservableCollection<Point>(CalculatePoints());

        OnPositionChanged += _ => Points = new ObservableCollection<Point>(CalculatePoints());
    }

    public override PolygonType PolygonType => PolygonType.Triangle;

    public ObservableCollection<Point> Points { get; private set; }

    protected override IImmutableSolidColorBrush ShapeColor => Brushes.DarkOrange;

    private Point[] CalculatePoints()
    {
        var points = new Point[3];
        double radius = ShapeSize / Math.Sqrt(3);

        // 90°, 210°, 330° in radians
        double[] angles = [3 * Math.PI / 2, Math.PI / 6, 5 * Math.PI / 6];

        for (var i = 0; i < 3; i++)
        {
            double angle = angles[i];

            double x = radius * Math.Cos(angle) + ShapeSize / 2;
            double y = radius * Math.Sin(angle) + ShapeSize / 2;
            points[i] = new Point(x, y);
        }

        return points;
    }
}