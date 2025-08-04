using Avalonia;
using Avalonia.Media;

namespace ShapePuzzle.Models.Shapes;

public sealed class Circle(ShapeType type, Point position) : Shape(type, position)
{
    protected override IImmutableSolidColorBrush ShapeColor => Brushes.ForestGreen;
    public override PolygonType PolygonType => PolygonType.Circle;
}