using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Media;

namespace ShapePuzzle.Models;

public abstract class Shape(ShapeType type, Point position) : INotifyPropertyChanged
{
    public delegate void PositionChangedEvent(Point newPosition);

    private const int MovementSpeed = 20;

    private Point _centerPosition = position;

    private bool _isSelected;

    public double ShapeSize => 50;

    public Point ShapeCenter
    {
        get => _centerPosition;
        set
        {
            if (!SetField(ref _centerPosition, value)) return;


            OnPropertyChanged(nameof(PositionTop));
            OnPropertyChanged(nameof(PositionLeft));

            OnPositionChanged?.Invoke(value);
        }
    }

    public bool IsSelected
    {
        get => _isSelected && ShapeType == ShapeType.Movable;
        set
        {
            if (ShapeType != ShapeType.Movable) return;
            SetField(ref _isSelected, value);

            OnPropertyChanged(nameof(StrokeColor));
            OnPropertyChanged(nameof(StrokeWidth));
        }
    }

    public int StrokeWidth => ShapeType switch
    {
        ShapeType.Movable => 4,
        ShapeType.MovableInPosition => 0,
        ShapeType.Target => 2,
        _ => throw new ArgumentOutOfRangeException()
    };

    public IImmutableSolidColorBrush? StrokeColor => ShapeType switch
    {
        ShapeType.Movable => IsSelected ? Brushes.Gold : null,
        ShapeType.MovableInPosition => null,
        ShapeType.Target => Brushes.Fuchsia,
        _ => throw new ArgumentOutOfRangeException()
    };


    public ShapeType ShapeType { get; set; } = type;

    public double PositionLeft => ShapeCenter.X - ShapeSize / 2;

    public double PositionTop => ShapeCenter.Y - ShapeSize / 2;

    public IImmutableSolidColorBrush FillColor => ShapeType == ShapeType.Target ? Brushes.Transparent : ShapeColor;


    protected abstract IImmutableSolidColorBrush ShapeColor { get; }

    public abstract PolygonType PolygonType { get; }
    public event PropertyChangedEventHandler? PropertyChanged;

    public event PositionChangedEvent? OnPositionChanged;

    public void Move(MovementDirection movementDirection)
    {
        var (x, y) = ShapeCenter;

        switch (movementDirection)
        {
            case MovementDirection.Up: y -= MovementSpeed; break;
            case MovementDirection.Down: y += MovementSpeed; break;
            case MovementDirection.Left: x -= MovementSpeed; break;
            case MovementDirection.Right: x += MovementSpeed; break;
            default:
                throw new ArgumentOutOfRangeException(nameof(movementDirection), movementDirection, null);
        }

        ShapeCenter = new Point(x, y);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}