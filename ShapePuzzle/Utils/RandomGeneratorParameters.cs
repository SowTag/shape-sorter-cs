namespace ShapePuzzle.Utils;

public record RandomGeneratorParameters(
    int ShapeCountMin,
    int ShapeCountMax,
    int ShapePositionMinX,
    int ShapePositionMinY,
    int ShapePositionMaxX,
    int ShapePositionMaxY
);