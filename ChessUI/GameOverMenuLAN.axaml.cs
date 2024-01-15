using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace ChessUI;

public partial class GameOverMenuLAN : UserControl
{
    public event Action<Option> OptionSelected;
    public GameOverMenuLAN()
    {
        InitializeComponent();
    }

    private void BackToMenu_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.BackToMainMenu);
    }
}