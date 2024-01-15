namespace SocketProject
{
    [Serializable]
    public enum SocketCommand
    {
        SEND_GAME_STATE,
        NEW_GAME,
        QUIT,
        SEND_SERVER_PIECE,
        SERVER_PIECE_REQUEST,
    }

}
