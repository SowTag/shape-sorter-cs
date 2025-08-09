using System;
using Avalonia.Controls;
using Avalonia.Input;
using ShapePuzzle.Models;
using ShapePuzzle.ViewModels;

namespace ShapePuzzle.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void GameCanvas_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (DataContext is not MainWindowViewModel vm) return;


        if (vm.IsGameOver) return;

        var isShiftPressed = (e.KeyModifiers & KeyModifiers.Shift) != 0;
        
        switch (e.Key)
        {
            case Key.Tab: vm.SelectShapeDelta(isShiftPressed ? -1 : 1); break;

            case Key.Up or Key.W: vm.MoveCurrentShape(MovementDirection.Up); break;
            case Key.Down or Key.S: vm.MoveCurrentShape(MovementDirection.Down); break;
            case Key.Left or Key.A: vm.MoveCurrentShape(MovementDirection.Left); break;
            case Key.Right or Key.D: vm.MoveCurrentShape(MovementDirection.Right); break;

            case Key.Space: vm.CheckShapeTarget(); break;
        }
    }

    private void GameCanvas_OnInitialized(object? sender, EventArgs e)
    {
        if (sender is Canvas canvas)
        {
            canvas.Focus();
        }
    }
}