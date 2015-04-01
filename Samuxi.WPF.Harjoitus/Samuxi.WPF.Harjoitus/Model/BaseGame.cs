using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Model
{
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
            }
        }

        #endregion

        #region  Methods

        /// <summary>
        /// Determines whether [is valid movement] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public abstract bool IsValidMovement(BoardItem item, System.Windows.Point point);


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
                if (oppositeItem.Side == PlayerSide.None)
                {
                    oppositeItem.Row = boardItem.Row;
                    oppositeItem.Column = boardItem.Column;
                }
                else
                {
                    BoardItems.Add(CreateDummyBoardItem(oppositeItem.GamePosition));
                    BoardItems.Remove(oppositeItem);
                }
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

        /// <summary>
        /// Loads the game with given board items.
        /// </summary>
        /// <param name="items">The board items.</param>
        /// <returns>Is load was succesfull</returns>
        public bool LoadGame(ObservableCollection<BoardItem> items)
        {
            if (items != null)
            {
                BoardItems = items;
                CreateGame();

                return true;
            }

            return false;
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
