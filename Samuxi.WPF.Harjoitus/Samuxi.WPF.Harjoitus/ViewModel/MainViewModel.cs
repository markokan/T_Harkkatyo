using System.Windows.Media;
using GalaSoft.MvvmLight;
using Samuxi.WPF.Harjoitus.Model;

namespace Samuxi.WPF.Harjoitus.ViewModel
{

    /// <summary>
    /// Main window ViewModel.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        private IGame _currentGame;
        /// <summary>
        /// Gets or sets the current game.
        /// </summary>
        /// <value>
        /// The current game.
        /// </value>
        public IGame CurrentGame
        {
            get { return _currentGame; }
            set
            {
                _currentGame = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            //CurrentGame = new BreakthroughGame();
            CurrentGame = new CheckerGame
            {
                Size = new GameSize {Columns = 8, Rows = 8},
                PlayerWhite = new Player
                {
                    Name = "Matti",
                    SymbolColor = Colors.Brown,
                    Side = PlayerSide.WhiteSide,
                    MarkerSymbol = MarkerSymbol.Ellipse
                },
                PlayerBlack = new Player
                {
                    Name = "Pekka",
                    SymbolColor = Colors.BurlyWood,
                    Side = PlayerSide.BlackSide,
                    MarkerSymbol = MarkerSymbol.Triangle
                }
            };

            CurrentGame.CreateGame();
        }
    }
}