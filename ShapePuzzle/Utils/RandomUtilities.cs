using System;
using Avalonia;
using ShapePuzzle.Models;
using ShapePuzzle.Models.Shapes;

namespace ShapePuzzle.Utils;

public class RandomUtilities(RandomGeneratorParameters parameters)
{
    private readonly Random _randomNumberGenerator = new();

    private static T GenerateRandomEnumValue<T>(Random random) where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length))!;
    }

    public Shape[] GenerateRandomShapes()
    {
        var shapeCount = _randomNumberGenerator.Next(parameters.ShapeCountMin, parameters.ShapeCountMax);

        var generatedShapes = new Shape[shapeCount];

        for (var i = 0; i < shapeCount; i++)
        {
            var shapeType = GenerateRandomEnumValue<PolygonType>(_randomNumberGenerator);
            var shapePosition = GenerateRandomPointWithinShapeBounds();

            Shape shape = shapeType switch
            {
                PolygonType.Circle => new Circle(ShapeType.Movable, shapePosition),
                PolygonType.Triangle => new Triangle(ShapeType.Movable, shapePosition),
                PolygonType.Square => new Square(ShapeType.Movable, shapePosition),
                _ => throw new ArgumentOutOfRangeException()
            };

            generatedShapes[i] = shape;
        }

        return generatedShapes;
    }

    private Point GenerateRandomPointWithinShapeBounds()
    {
        var x = GenerateRandomDoubleWithinBounds(parameters.ShapePositionMinX, parameters.ShapePositionMaxX);
        var y = GenerateRandomDoubleWithinBounds(parameters.ShapePositionMinY, parameters.ShapePositionMaxY);

        return new Point(x, y);
    }

    private double GenerateRandomDoubleWithinBounds(double min, double max)
    {
        return _randomNumberGenerator.NextDouble() * (max - min) + min;
    }
}