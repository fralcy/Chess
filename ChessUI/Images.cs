using Avalonia.Media.Imaging;
using ChessLogic;
using System.Collections.Generic;

namespace ChessUI
{
    public static class Images
    {
        private static readonly Dictionary<PieceType, Bitmap> whiteSources = new()
        {
            {PieceType.Pawn, LoadImage("Assets/PawnW.png") },
            {PieceType.Bishop, LoadImage("Assets/BishopW.png")},
            {PieceType.Knight, LoadImage("Assets/KnightW.png") },
            {PieceType.Rook, LoadImage("Assets/RookW.png") },
            {PieceType.King, LoadImage("Assets/KingW.png")},
            {PieceType.Queen, LoadImage("Assets/QueenW.png") }
        };
        private static readonly Dictionary<PieceType, Bitmap> blackSources = new()
        {
            {PieceType.Pawn, LoadImage("Assets/PawnB.png") },
            {PieceType.Bishop, LoadImage("Assets/BishopB.png")},
            {PieceType.Knight, LoadImage("Assets/KnightB.png") },
            {PieceType.Rook, LoadImage("Assets/RookB.png") },
            {PieceType.King, LoadImage("Assets/KingB.png")},
            {PieceType.Queen, LoadImage("Assets/QueenB.png") }
        };
        private static Bitmap LoadImage(string filePath)
        {
            return new Bitmap(filePath);
        }
        public static Bitmap GetImage(Player color, PieceType type)
        {
            return color switch
            {
                Player.White => whiteSources[type],
                Player.Black => blackSources[type],
                _ => null
            };
        }
        public static Bitmap GetImage(Piece piece)
        {
            if (piece == null)
            {
                return null;
            }
            return GetImage(piece.Color, piece.Type);
        }
    }
}