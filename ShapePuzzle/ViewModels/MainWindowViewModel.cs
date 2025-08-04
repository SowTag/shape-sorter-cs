using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using ShapePuzzle.Models;
using ShapePuzzle.Models.Shapes;
using ShapePuzzle.Utils;

namespace ShapePuzzle.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    private const int ShapeToTargetDistanceTolerance = 25;

    private readonly RandomGeneratorParameters _randomGeneratorParameters = new(
        5,
        15,
        100,
        ShapePositionMaxX: 1100,
        ShapePositionMinY: 300,
        ShapePositionMaxY: 700);

    private readonly Shape[] _shapeTargets =
    [
        new Circle(ShapeType.Target, new Point(300, 100)),
        new Triangle(ShapeType.Target, new Point(600, 100)),
        new Square(ShapeType.Target, new Point(900, 100))
    ];

    private bool _isGameOver;

    private int _selectedShape;

    private Shape[] _shapes = [];

    public MainWindowViewModel()
    {
        var randomUtilities = new RandomUtilities(_randomGeneratorParameters);

        Shape[] generatedShapes = randomUtilities.GenerateRandomShapes();

        Shape[] combinedShapeArray = generatedShapes.Concat(_shapeTargets).ToArray();

        Shapes = new ObservableCollection<Shape>(combinedShapeArray);
        SelectedShape = 0;
    }

    public double GameOverOverlayOpacity => IsGameOver ? 1 : 0;

    public bool IsGameOver
    {
        get => _isGameOver;
        set => SetProperty(ref _isGameOver, value, nameof(GameOverOverlayOpacity));
    }

    private int SelectedShape
    {
        get => _selectedShape;
        set
        {
            _selectedShape = value;

            UnselectAllShapes();

            if (RemainingShapes.Count > 0) RemainingShapes[value].IsSelected = true;

            OnPropertyChanged();
            OnPropertyChanged(nameof(Shapes));
            OnPropertyChanged(nameof(RemainingShapes));
        }
    }

    public ObservableCollection<Shape> Shapes
    {
        get => new(_shapes.Where(shape => shape.ShapeType != ShapeType.MovableInPosition).ToArray());
        set
        {
            _shapes = value.ToArray();

            OnPropertyChanged();
            OnPropertyChanged(nameof(RemainingShapes));
        }
    }


    public ObservableCollection<Shape> RemainingShapes =>
        new(Shapes.Where(shape => shape.ShapeType == ShapeType.Movable));

    private void UnselectAllShapes()
    {
        foreach (Shape shape in _shapes) shape.IsSelected = false;
    }

    public void SelectShapeDelta(int delta)
    {
        int wrappedIndex = (SelectedShape + delta) % RemainingShapes.Count;

        if (wrappedIndex < 0)
            wrappedIndex += RemainingShapes.Count;

        SelectedShape = wrappedIndex;
    }

    public void MoveCurrentShape(MovementDirection movementDirection)
    {
        RemainingShapes[SelectedShape].Move(movementDirection);
        OnPropertyChanged(nameof(Shapes));
    }


    public void CheckShapeTarget()
    {
        Shape current = RemainingShapes[SelectedShape];
        Shape target = Shapes.First(shape =>
            shape.ShapeType == ShapeType.Target && shape.PolygonType == current.PolygonType);

        double distance = DistanceUtilities.CalculateDistanceBetween(current.ShapeCenter, target.ShapeCenter);

        if (distance < ShapeToTargetDistanceTolerance)
        {
            current.ShapeType = ShapeType.MovableInPosition;

            if (RemainingShapes.Count == 0) IsGameOver = true;

            SelectedShape = 0;
        }
    }
}