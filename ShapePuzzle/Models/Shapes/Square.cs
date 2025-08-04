using Avalonia;
using Avalonia.Media;

namespace ShapePuzzle.Models.Shapes;

public sealed class Square(ShapeType type, Point position) : Shape(type, position)
{
    protected override IImmutableSolidColorBrush ShapeColor => Brushes.AliceBlue;
    public override PolygonType PolygonType => PolygonType.Square;
}