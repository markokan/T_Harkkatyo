using System;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// <summary>
    /// Player model object.
    /// </summary>
    [Serializable]
    public class Player : BaseModel
    {
        private string _name;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private Color _symbolColor;
        /// <summary>
        /// Gets or sets the color of the symbol.
        /// </summary>
        /// <value>
        /// The color of the symbol.
        /// </value>
        public Color SymbolColor
        {
            get { return _symbolColor; }
            set
            {
                _symbolColor = value;
                OnPropertyChanged();
            }
        }

        private PlayerSide _playerSide;
        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        /// <value>
        /// The side.
        /// </value>
        public PlayerSide Side
        {
            get {  return _playerSide;}
            set
            {
                _playerSide = value;
                OnPropertyChanged();
            }
        }

        private MarkerSymbol _playerSymbol;
        /// <summary>
        /// Gets or sets the marker symbol.
        /// </summary>
        /// <value>
        /// The marker symbol.
        /// </value>
        public MarkerSymbol MarkerSymbol
        {
            get { return _playerSymbol; }
            set
            {
                _playerSymbol = value;
                OnPropertyChanged();
            }
        }

        private PlayerType _playerType;
        /// <summary>
        /// Gets or sets the type of the player (Human or Computer -> Default is Human).
        /// </summary>
        /// <value>
        /// The type of the player.
        /// </value>
        public PlayerType PlayerType
        {
            get {  return _playerType; }
            set
            {
                _playerType = value;
                OnPropertyChanged();
            }
        }
    }
}
