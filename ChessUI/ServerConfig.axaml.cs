using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace ChessUI;

public partial class ServerConfig : UserControl
{
    public event Action<Option> OptionSelected;

    public ServerConfig()
    {
        InitializeComponent();
    }

    private void BlackCheckBox_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.BlackChosen);
    }

    private void WhiteCheckBox_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.WhiteChosen);
    }

    private void Host_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.Host);
    }

    private void BackToMenu_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.BackToMainMenu);
    }
}