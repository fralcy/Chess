using Avalonia;
using Avalonia.Input;
using Avalonia.Media.Imaging;

namespace ChessUI
{
    public static class ChessCursors
    {
        public static readonly Cursor WhiteCursor = LoadCursor("Assets/CursorW.cur");
        public static readonly Cursor BlackCursor = LoadCursor("Assets/CursorB.cur");
        private static Cursor LoadCursor(string filePath)
        {
            Bitmap image = new Bitmap(filePath);
            PixelPoint hotSpot = new PixelPoint(1, 1);
            return new Cursor(image, hotSpot);
        }
    }
}
