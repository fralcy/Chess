namespace SocketProject
{
    [Serializable]
    public enum SocketCommand
    {
        SEND_GAME_STATE,
        NEW_GAME,
        NOTIFY,
        QUIT,
        DISPLAY_CHAT_TEXT,
        SEND_SERVER_PIECE,
        SERVER_PIECE_REQUEST,
    }

}
