using System;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// <summary>
    /// Game settings object.
    /// </summary>
    [Serializable]
    public class GameSetting : BaseModel
    {
        private GameType _typeOfGame;
        /// <summary>
        /// Gets or sets the type of game.
        /// </summary>
        /// <value>
        /// The type of game.
        /// </value>
        public GameType TypeOfGame
        {
            get {  return _typeOfGame; }
            set
            {
                _typeOfGame = value;
                OnPropertyChanged();
            }
        }

        private GameSize _size;
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public GameSize Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }

        private Color _colorOne;

        /// <summary>
        /// Gets or sets the grid color one.
        /// </summary>
        /// <value>
        /// The grid color one.
        /// </value>
        public Color GridColorOne
        {
            get { return _colorOne;}
            set
            {
                _colorOne = value;
                OnPropertyChanged();
            }
        }

        private Color _colorTwo;
        /// <summary>
        /// Gets or sets the grid color two.
        /// </summary>
        /// <value>
        /// The grid color two.
        /// </value>
        public Color GridColorTwo
        {
            get { return _colorTwo;}
            set
            {
                _colorTwo = value;
                OnPropertyChanged();
            }
        }

        private Color _playerOneColor;
        /// <summary>
        /// Gets or sets the color of the player one.
        /// </summary>
        /// <value>
        /// The color of the player one.
        /// </value>
        public Color PlayerOneColor
        {
            get { return _playerOneColor; }
            set
            {
                _playerOneColor = value;
                OnPropertyChanged();
            }
        }

        private Color _playerTwoColor;
        /// <summary>
        /// Gets or sets the color of the player two.
        /// </summary>
        /// <value>
        /// The color of the player two.
        /// </value>
        public Color PlayerTwoColor
        {
            get { return _playerTwoColor; }
            set
            {
                _playerTwoColor = value;
                OnPropertyChanged();
            }
        }

        private MarkerSymbol _playerOneSymbol;
        /// <summary>
        /// Gets or sets the player one symbol.
        /// </summary>
        /// <value>
        /// The player one symbol.
        /// </value>
        public MarkerSymbol PlayerOneSymbol
        {
            get { return _playerOneSymbol; }
            set
            {
                _playerOneSymbol = value;
                OnPropertyChanged();
            }
        }

        private MarkerSymbol _playerTwoSymbol;
        /// <summary>
        /// Gets or sets the player two symbol.
        /// </summary>
        /// <value>
        /// The player two symbol.
        /// </value>
        public MarkerSymbol PlayerTwoSymbol
        {
            get { return _playerTwoSymbol; }
            set
            {
                _playerTwoSymbol = value;
                OnPropertyChanged();
            }
        }

    }
}
