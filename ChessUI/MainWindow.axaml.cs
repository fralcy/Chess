using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using ChessLogic;
using Microsoft.CodeAnalysis;
using SocketProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace ChessUI
{
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();
        private GameState gameState;
        private Position selectedPos = null;
        private DispatcherTimer timer = new DispatcherTimer();
        private SocketManager socket;
        private LoginMenu loginMenu;
        private Board board = new Board();
        private bool isLANRun = false, isBotActive = false;
        private Player player;
        private string serverPieceMsg;
        private bool isServer = false, isClient = false;
        private Stack<GameState> gameStateStack = new();


        public MainWindow()
        {
            InitializeComponent();
            socket = new SocketManager();
            ShowLoginMenu();
            InitializeBoard();
            gameState = new GameState(Player.White, board.Initial());
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.IsEnabled = false;
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (gameState.whiteTimer.TotalSeconds != 0 && gameState.blackTimer.TotalSeconds != 0)
            {
                if (gameState.CurrentPlayer == Player.White)
                {
                    gameState.whiteTimerTick();
                    TimerTextBox.Text = gameState.whiteTimer.ToString(@"mm\:ss");
                }
                else
                {
                    gameState.blackTimerTick();
                    TimerTextBox2.Text = gameState.blackTimer.ToString(@"mm\:ss");
                }
            }
            else
            {
                if (gameState.whiteTimer.TotalSeconds == 0)
                {
                    gameState.Result = Result.Win(Player.Black, EndReason.Timeout);
                }
                else
                {
                    gameState.Result = Result.Win(Player.White, EndReason.Timeout);
                }
                ShowGameOver();
            }
        }

        private void InitializeBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Image image = new Image();
                    pieceImages[r, c] = image;
                    PieceGrid.Children.Add(image);
                    Rectangle highlight = new Rectangle();
                    highlights[r, c] = highlight;
                    HighlightGrid.Children.Add(highlight);
                }
            }
        }
        private void DrawBoard(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = board[r, c];
                    pieceImages[r, c].Source = Images.GetImage(piece); // call from invalid thread
                }
            }
        }
        private void BoardGrid_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (IsMenuOnScreen())
                return;


            if (isLANRun && player != gameState.CurrentPlayer)
                return;


            Point point = e.GetPosition(BoardGrid);
            Position pos = ToSquarePosition(point);

            if (selectedPos == null)
            {
                OnFromPostionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }


        }

        #region Function to redraw image on received

        async void RunSetImage(Image i, Piece p)
        {
            _ = Task.Run(() => OnImageFromAnotherThread(i, p));
        }

        /*
        void SetText(string text) => loginMenu.MessageTextBlock.Text = text;
        string GetText() => loginMenu.MessageTextBlock.Text ?? "";
        */
        async void OnImageFromAnotherThread(Image i, Piece p)
        {
            try
            {
                Dispatcher.UIThread.Post(() => SetImage(i, p));
                //var result = await Dispatcher.UIThread.InvokeAsync(GetImage);
            }
            catch (Exception)
            {
                throw;
            }
        }


        void SetImage(Image i, Piece p) => i.Source = Images.GetImage(p);
        //IImage GetImage(Image i) => i.Source ?? null;

        #endregion

        void DrawBoard_OtherPlayer(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = board[r, c];
                    //pieceImages[r, c].Source = Images.GetImage(piece); // call from invalid thread
                    RunSetImage(pieceImages[r, c], piece);
                }
            }
        }

        public void BoardGrid_OtherPlayerPointerPressed(GameState otherGameState)
        {
            gameState = otherGameState;
            //gameState.CurrentPlayer.Oppenent();
            DrawBoard_OtherPlayer(gameState.Board);
            if (gameState.IsGameOver())
            {
                ShowGameOver();
            }
            timer.Start();
            //isTurn = true;
        }

        private Position ToSquarePosition(Point point)
        {
            double squareSize = (BoardGrid.Bounds.Width / 8);
            int row = (int)(point.Y / squareSize);
            int col = (int)(point.X / squareSize);
            return new Position(row, col);
        }
        private void OnFromPostionSelected(Position pos)
        {
            IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos);
            if (moves.Any())
            {
                selectedPos = pos;
                CacheMoves(moves);
                ShowHighlights();
            }
        }
        private void OnToPositionSelected(Position pos)
        {
            selectedPos = null;
            HideHighlights();

            if (moveCache.TryGetValue(pos, out Move move))
            {
                if (move.Type == MoveType.PawnPromotion)
                {
                    HandlePromotion(move.FromPos, move.ToPos);
                }
                else
                {
                    HandleMove(move);
                }
            }

        }
        private void HandlePromotion(Position from, Position to)
        {
            pieceImages[to.Row, to.Column].Source = Images.GetImage(gameState.CurrentPlayer, PieceType.Pawn);
            pieceImages[from.Row, from.Column].Source = null;

            PromotionMenu promMenu = new PromotionMenu(gameState.CurrentPlayer);
            MenuContainer.Content = promMenu;

            promMenu.PieceSelected += type =>
            {
                MenuContainer.Content = null;
                Move promMove = new PawnPromotion(from, to, type);
                HandleMove(promMove);
            };
        }

        void SubHandleMove(Move move)
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
        }
        private void HandleMove(Move move)
        {
            SubHandleMove(move);
            timer.Start();


            if (gameState.IsGameOver())
            {
                ShowGameOver();
            }
            if (isLANRun)
            {
                socket.Send(new SocketData((int)SocketCommand.SEND_GAME_STATE, "", gameState));
                Listen();
                //BoardGrid.IsEnabled = false;
            }

            if(isBotActive)
            {
                BotMakeMove();
            }

        }

        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();
            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;
            }
        }
        private void ShowHighlights()
        {
            Color color = Color.FromArgb(255, 144, 238, 144);
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);
            }
        }
        private void HideHighlights()
        {
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = Brushes.Transparent;
            }
        }
        private void SetCursor(Player player)
        {
            if (player == Player.White)
            {
                Cursor = ChessCursors.WhiteCursor;
            }
            else
            {
                Cursor = ChessCursors.BlackCursor;
            }
        }
        private bool IsMenuOnScreen()
        {
            return MenuContainer.Content != null;
        }
        private void ShowGameOver()
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                GameOverMenu gameOverMenu = new GameOverMenu(gameState);
                MenuContainer.Content = gameOverMenu;
                gameOverMenu.OptionSelected += option =>
                {
                    if (option == Option.Restart)
                    {
                        MenuContainer.Content = null;
                        RestartGame();
                    }
                    else
                    {
                        Close();
                    }
                };
            });
        }

        private void RestartGame()
        {
            timer.Stop();
            selectedPos = null;
            HideHighlights();
            moveCache.Clear();
            gameState = new GameState(Player.White, board.Initial());
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
            TimerTextBox.Text = gameState.whiteTimer.ToString(@"mm\:ss");
            TimerTextBox2.Text = gameState.blackTimer.ToString(@"mm\:ss");
            if (isLANRun && !IsMenuOnScreen())
            {
                socket.Send(new SocketData((int)SocketCommand.SEND_GAME_STATE, "", gameState));
                if (player == Player.Black)
                {
                    Listen();
                }
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsMenuOnScreen() && e.Key == Key.Escape)
            {
                ShowPauseMenu();
            }
        }
        private void ShowPauseMenu()
        {
            PauseMenu pauseMenu = new PauseMenu();
            MenuContainer.Content = pauseMenu;
            pauseMenu.OptionSelected += option =>
            {
                MenuContainer.Content = null;
                if (option == Option.Restart)
                {
                    RestartGame();
                }
                else if (option == Option.BackToMainMenu)
                {
                    RestartGame();
                    isLANRun = false;
                    isBotActive = false;
                    ShowLoginMenu();
                }
            };
        }

        private void ShowLANOptionMenu()
        {
            //isLANRun = true;
            LANOption lanOption = new LANOption();
            MenuContainer.Content = lanOption;
            lanOption.OptionSelected += option =>
            {
                if (option == Option.Join)
                {
                    //player = Player.Black;
                    socket = new SocketManager();
                    isClient = true;
                    isLANRun = true;
                    socket.IP = lanOption.IPTextBox.Text;
                    socket.ConnectServer();
                    socket.Send(new SocketData((int)SocketCommand.SERVER_PIECE_REQUEST, "", null));

                    //socket.ConnectServer();
                    Listen();
                    /*
                    if(socket.ConnectServer())
                        socket.Send(new SocketData((int)SocketCommand.SERVER_PIECE_REQUEST, "", null));
                    */
                    //player = Player.Black;
                    MenuContainer.Content = null;
                    //ShowGameLobbyMenu();
                    //glb.p2.Text = lanOption.NicknameTextBox.Text;
                }
                else if (option == Option.Host)
                {

                    //socket.IP = loginMenu.IPTextBox.Text;
                    //player = Player.White;
                    ShowServerConfig();
                    //socket.CreateServer();
                    //ShowGameLobbyMenu();
                    //Listen();
                    //glb.p1.Text = lanOption.NicknameTextBox.Text;
                }
                else if (option == Option.BackToMainMenu)
                {
                    RestartGame();
                    isLANRun = false;
                    ShowLoginMenu();
                }
            };
        }

        void ShowServerConfig()
        {
            ServerConfig serverConfig = new ServerConfig();

            MenuContainer.Content = serverConfig;
            serverConfig.IPTextBlock.Text = "Your IP: " + loginMenu.IPTextBox.Text;
            serverConfig.OptionSelected += option =>
            {
                if (option == Option.BlackChosen)
                {
                    if (serverConfig.WhiteCheckBox.IsChecked == false)
                        serverConfig.BlackCheckBox.IsChecked = true;
                    serverConfig.WhiteCheckBox.IsChecked = false;
                    player = Player.Black;
                }
                else if (option == Option.WhiteChosen)
                {
                    if (serverConfig.BlackCheckBox.IsChecked == false)
                        serverConfig.WhiteCheckBox.IsChecked = true;
                    serverConfig.BlackCheckBox.IsChecked = false;
                    player = Player.White;
                }
                else if (option == Option.Host)
                {
                    isServer = true;
                    isLANRun = true;
                    //player = Player.Black;
                    //gameState.CurrentPlayer = player;
                    socket = new SocketManager();
                    socket.IP = loginMenu.IPTextBox.Text;
                    // Lan run here
                    if (!socket.ConnectServer())
                        socket.CreateServer();
                    //serverConfig.IsEnabled = false;
                    //player = Player.White;

                    if (serverConfig.WhiteCheckBox.IsChecked == true)
                    {
                        player = Player.White;
                        serverPieceMsg = "White";
                    }

                    if (serverConfig.BlackCheckBox.IsChecked == true)
                    {
                        player = Player.Black;
                        serverPieceMsg = "Black";
                    }
                    //gameState.CurrentPlayer = player;
                    /*
                    if(serverConfig.BlackCheckBox.IsChecked == true)
                    {
                        gameState.CurrentPlayer = Player.Black;
                    }
                    */

                    Thread.Sleep(3000);
                    Listen();
                    MenuContainer.Content = null;

                    //player = Player.White;
                }
            };
        }

        private void ShowLoginMenu()
        {
            loginMenu = new LoginMenu();
            loginMenu.IPTextBox.Text = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            if (string.IsNullOrEmpty(loginMenu.IPTextBox.Text))
            {
                loginMenu.IPTextBox.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }

            MenuContainer.Content = loginMenu;
            loginMenu.OptionSelected += option =>
            {
                MenuContainer.Content = null;
                if (option == Option.Start)
                {
                    //player = null;
                    RestartGame();
                }
                else if (option == Option.LAN)
                {
                    isLANRun = true;
                    ShowLANOptionMenu();
                    //LANRun();
                }
                else if (option == Option.PlayAgainstAI)
                {
                    player = Player.White;
                    isBotActive = true;
                }
            };
        }

        #region LAN goes here

        void Listen()
        {
            try
            {
                Thread listenThread = new Thread(() =>
                {
                    SocketData data = socket.Receive();
                    //_ = Task.Run(() => OnTextFromAnotherThread(data)); // data can be replaced by whatever u want
                    ProcessData(data);
                });

                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch
            {
                ShowGameOverLAN();
            }

        }

        void ShowGameOverLAN()
        {

        }

        /*
        async void ShowMessage(string message)
        {
            _ = Task.Run(() => OnTextFromAnotherThread(message));
        }
        
        
        void SetText(string text) => loginMenu.MessageTextBlock.Text = text;
        string GetText() => loginMenu.MessageTextBlock.Text ?? "";
        
        async void OnTextFromAnotherThread(string text)
        {
            try
            {
                Dispatcher.UIThread.Post(() => SetText(text));
                var result = await Dispatcher.UIThread.InvokeAsync(GetText);
            }
            catch (Exception)
            {
                throw;
            }
        }
        */

        void ProcessData(SocketData data)
        {
            switch (data.Command)
            {
                case (int)SocketCommand.NOTIFY:
                    //ShowMessage(data.Message);
                    break;
                case (int)SocketCommand.SEND_GAME_STATE:
                    BoardGrid_OtherPlayerPointerPressed(data.GameState);
                    break;
                case (int)SocketCommand.QUIT:
                    break;
                case (int)SocketCommand.DISPLAY_CHAT_TEXT:
                    DisplayChatText(data.Message);
                    break;
                case (int)SocketCommand.SEND_SERVER_PIECE:
                    SetClientPiece(data.Message);
                    break;
                case (int)SocketCommand.SERVER_PIECE_REQUEST:
                    SendServerPiece();
                    break;
                default:
                    break;
            }

            Listen();
        }

        void SendServerPiece()
        {
            socket.Send(new SocketData((int)SocketCommand.SEND_SERVER_PIECE, serverPieceMsg, null));
        }

        void SetClientPiece(string pieceType)
        {
            if (isServer == false && isClient == true && pieceType == "Black")
            {
                player = Player.White;
                //gameState.CurrentPlayer = Player.White;
            }
            if (isServer == false && isClient == true && pieceType == "White")
            {
                player = Player.Black;
                //gameState.CurrentPlayer = Player.Black;
            }

        }


        void DisplayChatText(string msg)
        {
           
        }
        #endregion


        #region Bot make move
        private void BotMakeMove()
        {
            GameState copyGameState = gameState.Copy();
            Move bestMove = MiniMaxRoot(3, copyGameState, false);
            SubHandleMove(bestMove);
            timer.Start();


            if (gameState.IsGameOver())
            {
                ShowGameOver();
            }
        }

        private Move MiniMaxRoot(int depth, GameState gameState, bool isMaximisingPlayer)
        {
            /*
            Board boardCopy = board.Copy();
            GameState gameStateCopy = new GameState(gameState.CurrentPlayer, boardCopy);
            */
            IEnumerable<Move> movesCollection = gameState.AllLegalMovesFor(gameState.CurrentPlayer); // get all moves posible
            List<Move> moveList = movesCollection.ToList();
            double bbestMove = -9999;
            Move bestMoveFound = new Move();
            for (int i = 0; i < moveList.Count; i++)
            {
                Move newGameMove = moveList[i];
                GameState newGameState = gameState.Copy();
                gameStateStack.Push(newGameState.Copy());
                newGameState.MakeMove(newGameMove);
                double value = MiniMax(depth - 1, newGameState, -10000, 10000, isMaximisingPlayer);
                if (gameStateStack.Count > 0)
                    newGameState = gameStateStack.Pop();
                //newGameState = gameStateStack.Pop();
                if (value >= bbestMove)
                {
                    bbestMove = value;
                    bestMoveFound = newGameMove;
                }
            }


            return bestMoveFound;

        }

        private double MiniMax(int depth, GameState gameState, double alpha, double beta, bool isMaxmisingPlayer)
        {
            if (depth == 0 || gameState.IsGameOver())
            {
                board.SetValue();
                double ret = board.BoardValue;
                //int deb = 10;
                return -ret;
            }

            if (isMaxmisingPlayer)
            {
                double bestMove = -9999;
                IEnumerable<Move> movesCollection = gameState.AllLegalMovesFor(player); // get all moves posible
                List<Move> moveList = movesCollection.ToList();
                for (int i = 0; i < moveList.Count; i++)
                {
                    //gameStateStack.Push(gameState);
                    Move newGameMove = moveList[i];
                    GameState newGameState = gameState.Copy();
                    gameStateStack.Push(newGameState);
                    newGameState.MakeMove(newGameMove);
                    double value = MiniMax(depth - 1, newGameState, alpha, beta, false);
                    bestMove = Math.Max(bestMove, value);

                    if (gameStateStack.Count > 0)
                    {
                        newGameState = gameStateStack.Pop();
                    }
                    alpha = Math.Max(alpha, bestMove);
                    if (beta <= alpha)
                        return bestMove;
                }

                return bestMove;
            }

            else
            {
                Player p2 = gameState.CurrentPlayer.Oppenent();
                double bestMove = 9999;
                IEnumerable<Move> movesCollection = gameState.AllLegalMovesFor(p2); // get all moves posible
                List<Move> moveList;
                moveList = movesCollection.ToList();
                for (int i = 0; i < moveList.Count; i++)
                {
                    gameStateStack.Push(gameState.Copy());
                    Move newGameMove = moveList[i];
                    GameState newGameState = gameState.Copy();
                    newGameState.MakeMove(newGameMove);
                    double value = MiniMax(depth - 1, newGameState, alpha, beta, true);
                    bestMove = Math.Min(bestMove, value);


                    if (gameStateStack.Count > 0)
                    {
                        newGameState = gameStateStack.Pop();
                    }
                    beta = Math.Min(beta, bestMove);
                    if (beta <= alpha)
                        return bestMove;
                }

                return bestMove;
            }
        }

        #endregion

    }

}