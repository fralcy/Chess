using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace ChessUI;

public partial class LANOption : UserControl
{
    public event Action<Option> OptionSelected;
    public LANOption()
    {
        InitializeComponent();
    }

    private void Join_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.Join);
    }

    private void Host_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.Host);
    }

    private void BackToMainMenu_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.BackToMainMenu);
    }
}