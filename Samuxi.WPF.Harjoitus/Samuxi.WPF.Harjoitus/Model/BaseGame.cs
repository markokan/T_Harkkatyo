using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Threading;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// @version 26.4.2015
    /// @author Marko Kangas
    /// 
    /// <summary>
    /// Base class to all Games.
    /// </summary>
    public abstract class BaseGame : IGame
    {
        #region Properties

        private PlayerSide _turn;

        /// <summary>
        /// Gets or sets the turn.
        /// </summary>
        /// <value>
        /// The turn.
        /// </value>
        public PlayerSide Turn
        {
            get { return _turn;}
            set
            {
                _turn = value;
                OnPropertyChanged();
                OnPropertyChanged("CurrentPlayer");
            }
        }

        private GameSize _gameSize;
        /// <summary>
        /// Gets or sets the game size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public GameSize Size
        {
            get { return _gameSize;}
            set
            {
                _gameSize = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<BoardItem> _boardItems;
        /// <summary>
        /// Gets or sets the board items.
        /// </summary>
        /// <value>
        /// The board items.
        /// </value>
        public ObservableCollection<BoardItem> BoardItems
        {
            get {  return _boardItems; }
            set {
                _boardItems = value; 
                OnPropertyChanged(); 
            }
        }

        private ObservableCollection<Move> _playedMoves;

        /// <summary>
        /// Gets or sets the played moves.
        /// </summary>
        /// <value>
        /// The played moves.
        /// </value>
        public ObservableCollection<Move> PlayedMoves
        {
            get {  return _playedMoves;}
            set
            {
                _playedMoves = value;
                OnPropertyChanged();
            }
        }

        private Player _playerBlack;
        /// <summary>
        /// Gets or sets the player black.
        /// </summary>
        /// <value>
        /// The player black.
        /// </value>
        public Player PlayerBlack
        {
            get { return _playerBlack; }
            set
            {
                _playerBlack = value;
                OnPropertyChanged();
            }
        }

        private Player _playerWhite;
        /// <summary>
        /// Gets or sets the player white.
        /// </summary>
        /// <value>
        /// The player white.
        /// </value>
        public Player PlayerWhite
        {
            get { return _playerWhite; }
            set
            {
                _playerWhite = value;
                OnPropertyChanged();
            }
        }

        private Player _winner;
        /// <summary>
        /// Gets or sets the winner player.
        /// </summary>
        /// <value>
        /// The winner.
        /// </value>
        public Player Winner
        {
            get
            {
                return _winner;

            }
            set
            {
                _winner = value;
                OnPropertyChanged();
                OnPropertyChanged("IsGameEnd");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is game end.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is game end; otherwise, <c>false</c>.
        /// </value>
        public bool IsGameEnd
        {
            get { return Winner != null; }
        }

        private GameSetting _setting;
        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public GameSetting Setting
        {
            get { return _setting;}
            set
            {
                _setting = value;
                OnPropertyChanged();
            }
        }

        private bool _isReplayRunning;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is replay running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is replay running; otherwise, <c>false</c>.
        /// </value>
        public bool IsReplayRunning
        {
            get { return _isReplayRunning; }
            set
            {
                _isReplayRunning = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGame"/> class.
        /// </summary>
        protected BaseGame()
        {
            PlayedMoves = new ObservableCollection<Move>();
        }


        #endregion

        #region  Methods

        /// <summary>
        /// Determines whether [is valid movement].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public abstract bool IsValidMovement(BoardItem item, System.Windows.Point point);

        /// <summary>
        /// Determines whether [is valid movement] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public abstract bool IsValidMovement(BoardItem item);

        /// <summary>
        /// Moves the specified board item.
        /// </summary>
        /// <param name="boardItem">The board item.</param>
        /// <param name="toPosition">To position.</param>
        public abstract void Move(BoardItem boardItem, GamePosition toPosition);


        /// <summary>
        /// Gets the possible moves.
        /// </summary>
        /// <param name="boardItem">The board item.</param>
        /// <returns>List of possible moves</returns>
        public abstract List<GamePosition> GetPossibleMoves(BoardItem boardItem);

        /// <summary>
        /// Gets all possible moves.
        /// </summary>
        /// <param name="side">The side.</param>
        /// <returns>possible moves</returns>
        public abstract List<Move> GetAllPossibleMoves(PlayerSide side);

        /// <summary>
        /// Creates the game.
        /// </summary>
        public abstract void CreateGame();

        /// <summary>
        /// Creates the dummy board items.
        /// </summary>
        public virtual void CreateDummyItems()
        {
            if (BoardItems == null)
            {
                BoardItems = new ObservableCollection<BoardItem>();    
            }

            for (int x = 0; x < Size.Columns; x++)
            {
                for (int y = 0; y < Size.Rows; y++)
                {
                    var position = new GamePosition {Column = x, Row = y};
                    var found = GetItem(position);

                    if (found == null)
                    {
                        BoardItems.Add(CreateDummyBoardItem(new GamePosition{ Column = x, Row = y}));
                    }
                }
            }
        }

        /// <summary>
        /// Creates the dummy board item.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public virtual BoardItem CreateDummyBoardItem(GamePosition position)
        {
            return new BoardItem
            {
                Column = position.Column,
                Row = position.Row,
                FillBrush = new SolidColorBrush(Colors.Transparent),
                Side = PlayerSide.None,
                Symbol = MarkerSymbol.Dummy
            };
        }

        /// <summary>
        /// Handles the board item move in collection.
        /// </summary>
        /// <param name="boardItem">The board item.</param>
        /// <param name="toPosition">To position.</param>
        public void HandleMove(BoardItem boardItem, GamePosition toPosition)
        {
            var oppositeItem = GetItem(toPosition, true);

            if (oppositeItem != null)
            {
                var move = new Move
                {
                    Id = boardItem.Id,
                    Position = toPosition,
                    Mover = Turn == PlayerSide.WhiteSide ? PlayerWhite : PlayerBlack,
                    ToId = oppositeItem.Id
                };

                if (oppositeItem.Side == PlayerSide.None)
                {
                    System.Diagnostics.Debug.WriteLine("{0} {1} to {2} {3}", oppositeItem.Row, oppositeItem.Column, boardItem.Row, boardItem.Column);                   
                    oppositeItem.Row = boardItem.Row;
                    oppositeItem.Column = boardItem.Column;
                }
                else
                {
                    var dummyItem = CreateDummyBoardItem(boardItem.GamePosition);

                    System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate 
                    {
                        BoardItems.Add(dummyItem);
                        BoardItems.Remove(oppositeItem);
                    });


                    move.AddedBoardItem = dummyItem;
                    move.RemovedBoardItem = oppositeItem;
                }

                System.Diagnostics.Debug.WriteLine(move.PrintFormat);

                PlayedMoves.Add(move);
            }
        }

        /// <summary>
        /// Clears the possible move items.
        /// </summary>
        public virtual void ClearPossibleMoveItems()
        {
            if (BoardItems != null)
            {
                foreach (var item in BoardItems.Where(c => c.IsPossibleMove))
                {
                    item.IsPossibleMove = false;
                }
            }


        }

        /// <summary>
        /// Replay this Game.
        /// </summary>
        public virtual void Replay()
        {
            
        }

        /// <summary>
        /// Gets the current board item.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="canReturnDummys">if set to <c>true</c> [can return dummys] default false.</param>
        /// <returns>
        /// BoardItem
        /// </returns>
        public BoardItem GetItem(GamePosition position, bool canReturnDummys = false)
        {
            if (BoardItems != null)
            {
                if (canReturnDummys)
                {
                    return BoardItems.FirstOrDefault(c => c.Column == position.Column && c.Row == position.Row);
                }

                return BoardItems.FirstOrDefault(c => c.Column == position.Column && c.Row == position.Row && c.Side != PlayerSide.None);
                
            }

            return null;
        }

        private Move UndoedMove { get; set; }

        /// <summary>
        /// Undoes last movement.
        /// </summary>
        /// <param name="isReplay">if set to <c>true</c> [is replay].</param>
        public void Undo(bool isReplay = false)
        {
            if (PlayedMoves != null)
            {
                var lastMovement = PlayedMoves.LastOrDefault();
                if (lastMovement != null)
                {
                    UndoedMove = lastMovement;

                    var itemMoved = BoardItems.FirstOrDefault(c => c.Id == lastMovement.Id);
                    if (itemMoved != null)
                    {
                        var dummyItem = GetItem(lastMovement.Position, true);
                        if (dummyItem != null)
                        {
                            dummyItem.Column = itemMoved.Column;
                            dummyItem.Row = itemMoved.Row;
                        }

                        itemMoved.Column = lastMovement.Position.Column;
                        itemMoved.Row = lastMovement.Position.Row;
                    }

                    if (lastMovement.RemovedBoardItem != null)
                    {
                        // palauta                    
                        BoardItems.Add(lastMovement.RemovedBoardItem);
                    }

                    if (lastMovement.AddedBoardItem != null)
                    {
                        BoardItems.Remove(lastMovement.AddedBoardItem);
                    }

                    if (!isReplay)
                    {
                        PlayedMoves.Remove(lastMovement);    
                    }
                    
                    Turn = Turn == PlayerSide.BlackSide ? PlayerSide.WhiteSide : PlayerSide.BlackSide;
                }
            }
        }

        /// <summary>
        /// Redoes last movement
        /// </summary>
        public void Redo()
        {
            if (UndoedMove != null)
            {
                if (PlayedMoves != null)
                {
                    var lastMovement = PlayedMoves.LastOrDefault();
                    if (lastMovement == null || lastMovement.Id == UndoedMove.Id)
                    {
                        var itemMoved = BoardItems.FirstOrDefault(c => c.Id == UndoedMove.Id);
                        if (itemMoved != null)
                        {
                            itemMoved.Column = UndoedMove.Position.Column;
                            itemMoved.Row = UndoedMove.Position.Row;
                        }

                        if (UndoedMove.RemovedBoardItem != null)
                        {                
                            BoardItems.Remove(UndoedMove.RemovedBoardItem);
                        }

                        if (UndoedMove.AddedBoardItem != null)
                        {
                            BoardItems.Add(UndoedMove.AddedBoardItem);
                        }

                        PlayedMoves.Add(UndoedMove);
                        Turn = Turn == PlayerSide.BlackSide ? PlayerSide.WhiteSide : PlayerSide.BlackSide;
                    }
                }
            }

            UndoedMove = null;
        }

        /// <summary>
        /// Gets the current player.
        /// </summary>
        /// <value>
        /// The current player.
        /// </value>
        public Player CurrentPlayer
        {
            get { return Turn == PlayerSide.WhiteSide ? PlayerWhite : PlayerBlack; }
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

       
    }
}
