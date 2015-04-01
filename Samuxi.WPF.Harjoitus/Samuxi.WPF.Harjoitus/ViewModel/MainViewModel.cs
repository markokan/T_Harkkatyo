using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Practices.ServiceLocation;
using Samuxi.WPF.Harjoitus.Model;
using Samuxi.WPF.Harjoitus.Views;

namespace Samuxi.WPF.Harjoitus.ViewModel
{

    /// <summary>
    /// Main window ViewModel.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Properties
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

        private GameSetting _currentGameSettings;
        /// <summary>
        /// Gets or sets the current game settings.
        /// </summary>
        /// <value>
        /// The current game settings.
        /// </value>
        public GameSetting CurrentGameSettings
        {
            get { return _currentGameSettings; }
            set
            {
                _currentGameSettings = value;
                RaisePropertyChanged();
            }
        }

#endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            RedoCommand = new RelayCommand(OnRedo);
            UndoCommand = new RelayCommand(OnUndo);
            OpenSettingsCommand = new RelayCommand(OnSettings);
            NewGameCommand = new RelayCommand(OnNewGame);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when [new game].
        /// </summary>
        private void OnNewGame()
        {
            if (CurrentGame != null)
            {
                // kysy nimet
                NewGameWindow view = new NewGameWindow();
                var result = view.ShowDialog();
                
                if (result.HasValue && result.Value)
                {
                    var vm = view.DataContext as NewGameViewModel;

                    if (vm != null)
                    {
                        InitGame(vm.PlayerOne, vm.PlayerTwo);

                        // Jos ladataan pit‰‰ asettaa....
                        CurrentGame.BoardItems = new ObservableCollection<BoardItem>();

                        CurrentGame.CreateGame();
                    }
                }
            }
        }

        /// <summary>
        /// Open settings window and handles changes.
        /// </summary>
        private void OnSettings()
        {
            var settingsViewModel = ServiceLocator.Current.GetInstance<SettingsViewModel>();
            if (settingsViewModel != null)
            {
                SettingsView view = new SettingsView();
                var vm = view.DataContext as SettingsViewModel;
                var result = view.ShowDialog();

                if (result.HasValue && result.Value && vm != null)
                {
                    CurrentGameSettings = vm.Settings;
                    InitGame("Player1", "Player2");
                }
            }
        }

        /// <summary>
        /// Initializes the game.
        /// </summary>
        /// <param name="nameOne">The name one.</param>
        /// <param name="nameTwo">The name two.</param>
        private void InitGame(string nameOne, string nameTwo)
        {
            if (CurrentGameSettings.TypeOfGame == GameType.BreakThrough)
            {
                CurrentGame = new BreakthroughGame
                {
                    Size = CurrentGameSettings.Size,
                    Setting = CurrentGameSettings,
                    PlayerBlack =
                        new Player
                        {
                            Name = nameOne,
                            MarkerSymbol = MarkerSymbol.Ellipse,
                            SymbolColor = CurrentGameSettings.PlayerTwoColor
                        },
                    PlayerWhite =
                        new Player
                        {
                            Name = nameTwo,
                            MarkerSymbol = MarkerSymbol.Ellipse,
                            SymbolColor = CurrentGameSettings.PlayerOneColor
                        }
                };
            }
            else
            {
                CurrentGame = new CheckerGame
                {
                    Size = CurrentGameSettings.Size,
                    Setting = CurrentGameSettings,
                    PlayerBlack =
                        new Player
                        {
                            Name = nameOne,
                            MarkerSymbol = MarkerSymbol.Ellipse,
                            SymbolColor = CurrentGameSettings.PlayerTwoColor
                        },
                    PlayerWhite =
                        new Player
                        {
                            Name = nameTwo,
                            MarkerSymbol = MarkerSymbol.Ellipse,
                            SymbolColor = CurrentGameSettings.PlayerOneColor
                        }
                };
            }
        }

        /// <summary>
        /// Called when [undo].
        /// </summary>
        private void OnUndo()
        {
            CurrentGame.Undo();
        }

        /// <summary>
        /// Called when [redo].
        /// </summary>
        private void OnRedo()
        {
            CurrentGame.Redo();
        }

#endregion

        #region Commands

        /// <summary>
        /// Gets the new game command.
        /// </summary>
        /// <value>
        /// The new game command.
        /// </value>
        public ICommand NewGameCommand { get; private set; }
        /// <summary>
        /// Gets the undo command.
        /// </summary>
        /// <value>
        /// The undo command.
        /// </value>
        public ICommand UndoCommand { get; private set; }
        /// <summary>
        /// Gets the redo command.
        /// </summary>
        /// <value>
        /// The redo command.
        /// </value>
        public ICommand RedoCommand { get; private set; }
        /// <summary>
        /// Gets the open settings command.
        /// </summary>
        /// <value>
        /// The open settings command.
        /// </value>
        public ICommand OpenSettingsCommand { get; private set; }

        #endregion
    }
}