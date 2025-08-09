using System;
using Avalonia;

namespace ShapePuzzle.Utils;

public static class DistanceUtilities
{
    public static double CalculateDistanceBetween(Point a, Point b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;

        return Math.Sqrt(dx * dx + dy * dy);
    }
}