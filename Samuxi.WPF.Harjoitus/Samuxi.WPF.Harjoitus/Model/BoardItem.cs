using System;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// <summary>
    /// Game board item. Handles board items.
    /// </summary>
    [Serializable]
    public class BoardItem : BaseModel
    {
        private readonly char[]  _colsChars = {'A','B','C','D','E','F','G','H','I','J','K','L','M'};

        private readonly SolidColorBrush _possibleBrush = new SolidColorBrush(Colors.GreenYellow);

        public string Id
        {
            get { return string.Format("{0}{1}", _colsChars[Column], Row); }
        }

        private PlayerSide _playerSide;
        /// <summary>
        /// Gets or sets the side of board item.
        /// </summary>
        /// <value>
        /// The side.
        /// </value>
        public PlayerSide Side
        {
            get {  return _playerSide; }
            set
            {
                _playerSide = value;
                OnPropertyChanged();
            }
        }

        private int _row;
        /// <summary>
        /// Gets or sets the row.
        /// </summary>
        /// <value>
        /// The row.
        /// </value>
        public int Row
        {
            get { return _row; }
            set
            {
                _row = value;
                OnPropertyChanged();
            }
            
        }

        private int _column;
        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>
        /// The column.
        /// </value>
        public int Column
        {
            get { return _column;}
            set
            {
                _column = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Gets the game position.
        /// </summary>
        /// <value>
        /// The game position.
        /// </value>
        public GamePosition GamePosition
        {
            get { return new GamePosition{ Column = Column, Row = Row}; }
        }

        private SolidColorBrush _fillBrush;
        /// <summary>
        /// Gets or sets the board item fill brush.
        /// </summary>
        /// <value>
        /// The fill brush.
        /// </value>
        public SolidColorBrush FillBrush
        {
            get
            {
                if (IsPossibleMove)
                    return _possibleBrush;
                
                return _fillBrush;
            }
            set
            {
                _fillBrush = value;
                OnPropertyChanged();
            }
        }

        private MarkerSymbol _symbol;
        /// <summary>
        /// Gets or sets the current board item symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public MarkerSymbol Symbol
        {
            get { return _symbol;}
            set
            {
                _symbol = value;
                OnPropertyChanged();
            }
        }

        private bool _isPossibleMove;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is possible move.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is possible move; otherwise, <c>false</c>.
        /// </value>
        public bool IsPossibleMove
        {
            get { return _isPossibleMove;}
            set
            {
                _isPossibleMove = value;
                OnPropertyChanged();
                OnPropertyChanged("FillBrush");
            }
        }

        private bool _isKing;
        /// <summary>
        /// Gets a value indicating whether this instance is king object.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is king; otherwise, <c>false</c>.
        /// </value>
        public bool IsKing
        {
            get { return _isKing; }
            set
            {
                _isKing = value;
                OnPropertyChanged();
            }
        }
    }
}
