using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace ChessUI;

public partial class LoginMenu : UserControl
{
    public event Action<Option> OptionSelected;
    public LoginMenu()
    {
        InitializeComponent();
    }

    private void NewGame_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.Start);
    }

    private void LAN_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.LAN);
    }

    private void AI_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.PlayAgainstAI);
    }
}