using Avalonia.Controls;
using Avalonia.Input;
using ChessLogic;
using System;

namespace ChessUI
{
    public partial class PromotionMenu : UserControl
    {
        public event Action<PieceType> PieceSelected;
        public PromotionMenu(Player player)
        {
            InitializeComponent();

            QueenImg.Source = Images.GetImage(player, PieceType.Queen);
            RookImg.Source = Images.GetImage(player, PieceType.Rook);
            BishopImg.Source = Images.GetImage(player, PieceType.Bishop);
            KnightImg.Source = Images.GetImage(player, PieceType.Knight);
        }
        private void QueenImg_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            PieceSelected?.Invoke(PieceType.Queen);
        }
        private void RookImg_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            PieceSelected?.Invoke(PieceType.Rook);
        }
        private void BishopImg_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            PieceSelected?.Invoke(PieceType.Bishop);
        }
        private void KnightImg_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            PieceSelected?.Invoke(PieceType.Knight);
        }
    }
}
