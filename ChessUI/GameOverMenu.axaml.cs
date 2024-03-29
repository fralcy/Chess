using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using ChessLogic;
using System;

namespace ChessUI
{
    public partial class GameOverMenu : UserControl
    {
        public event Action<Option> OptionSelected;
        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();

            Result result = gameState.Result;
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = GetReasonText(result.Reason, gameState.CurrentPlayer);
        }
        private static string GetWinnerText(Player winner)
        {
            return winner switch
            {
                Player.White => "WHITE WINS!",
                Player.Black => "BLACK WINS!",
                _ => "IT'S A DRAW!"
            } ;
        }
        private static string PlayerString(Player player)
        {
            return player switch
            {
                Player.White => "WHITE",
                Player.Black => "BLACK",
                _ => ""
            };
        }
        private static string GetReasonText(EndReason reason, Player currentPlayer)
        {
            return reason switch
            { 
                EndReason.Stalemate => $"STALEMATE - {PlayerString(currentPlayer)} CAN'T MOVE",
                EndReason.Checkmate => $"CHECKMATE - {PlayerString(currentPlayer)} CAN'T MOVE",
                EndReason.FiftyMoveRule => "FIFTY-MOVE RULE",
                EndReason.InsufficientMaterial => "INSUFFICIENT MATERIAL",
                EndReason.ThreefoldRepetion => "THREEFOLD REPETITION",
                EndReason.Timeout => $"TIMEOUT - {PlayerString(currentPlayer)}'S OUT OF TIME",
                _ => ""
            };
        }
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Restart);
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Exit);
        }
    }
}
