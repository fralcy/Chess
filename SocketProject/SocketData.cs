using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessLogic;

namespace SocketProject
{
    [Serializable]
    public class SocketData
    {
        private int command;

        public int Command { get => command; set => command = value; }

        private GameState gameState;

        public GameState GameState { get => gameState; set => gameState = value; }

        private string message;
        public string Message { get => message; set => message = value; }

        public SocketData(int command, string message, GameState gameState)
        {
            this.command = command;
            this.gameState = gameState;
            this.message = message;
        }
    }
}
