namespace ChessLogic
{
    public class Move
    {
        public virtual MoveType Type { get; }
        public virtual Position FromPos { get; }
        public virtual Position ToPos { get; }
        public virtual bool Execute(Board board)
        {
            return false;
        }
        public virtual bool IsLegal(Board board)
        {
            Player player = board[FromPos].Color;
            Board boardCopy = board.Copy();
            Execute(boardCopy);
            return !boardCopy.IsInCheck(player);
        }
    }
}
