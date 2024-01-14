namespace ChessLogic
{
    [Serializable]
    public class Queen : Piece
    {
        public override double Weight { get; }

        public override double[,] PosVal { get; }
        public override PieceType Type => PieceType.Queen;
        public override Player Color { get; }
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West,
            Direction.NorthEast,
            Direction.NorthWest,
            Direction.SouthEast,
            Direction.SouthWest
        };
        public Queen(Player color)
        {
            Color = color;
            PosVal = ps.evalQueen;
            if (Color == Player.White)
            {
                Weight = 90;
            }
            else
            {
                Weight = -90;
            }

        }
        public override Piece Copy()
        {
            Queen copy = new Queen(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
