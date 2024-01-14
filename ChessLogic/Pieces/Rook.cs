namespace ChessLogic
{
    [Serializable]
    public class Rook : Piece
    {
        public override double Weight { get; }
        public override double[,] PosVal { get; }
        public override PieceType Type => PieceType.Rook;
        public override Player Color { get; }
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West,
        };
        public Rook(Player color)
        {
            Color = color;
            if (Color == Player.White)
            {
                Weight = 90;
                PosVal = ps.rookEvalWhite;
            }
            else
            {
                Weight = -90;
                PosVal = ps.rookEvalBlack;
            }

        }
        public override Piece Copy()
        {
            Rook copy = new Rook(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
