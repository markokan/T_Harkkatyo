using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Practices.ServiceLocation;
using Samuxi.WPF.Harjoitus.Model;
using Samuxi.WPF.Harjoitus.Print;
using Samuxi.WPF.Harjoitus.Utils;
using Samuxi.WPF.Harjoitus.Views;
using Application = System.Windows.Forms.Application;
using System.Timers;
using System.Windows.Threading;


namespace Samuxi.WPF.Harjoitus.ViewModel
{
    /// <summary>
    /// Main window ViewModel.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        #endregion

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
            RedoCommand = new RelayCommand(OnRedo, CanRedo);
            UndoCommand = new RelayCommand(OnUndo, CanUndo);
            OpenSettingsCommand = new RelayCommand(OnSettings);
            NewGameCommand = new RelayCommand(OnNewGame);
            SaveGameCommand = new RelayCommand(OnSaveGame, CanSaveGame);
            LoadGameCommand = new RelayCommand(OnLoadGame);
            SaveResultCommand = new RelayCommand(OnSaveResult, CanSaveResult);
            PrintResultCommand = new RelayCommand<UIElement>(OnPrintResult, CanPrintResult);
            AboutCommand = new RelayCommand(OnOpenAbout);

            CurrentGameSettings = GameFileHandler.ReadSetting();
        }

       

        #endregion

        #region Methods

        /// <summary>
        /// Called when [open about].
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnOpenAbout()
        {
            var aboutViewModel = ServiceLocator.Current.GetInstance<AboutViewModel>();
            if (aboutViewModel != null)
            {
                AboutWindow view = new AboutWindow();
                var result = view.ShowDialog();

                if (result.HasValue && result.Value)
                {
                }
            }
        }

        /// <summary>
        /// Called when [print result].
        /// </summary>
        private void OnPrintResult(UIElement currentcontrol)
        {
            ScreenshotUtil.TakeScreenshot(currentcontrol);
            
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() != true) return;

            FlowDocument document = new FlowDocument
            {
                PageWidth = dialog.PrintableAreaWidth,
                PageHeight = dialog.PrintableAreaHeight
            };

            var imageControl = new Image { Source = new BitmapImage(new Uri(ScreenshotUtil.ScreenshotFilePath)) };

            document.Blocks.Add(new Paragraph(new InlineUIContainer(imageControl)));
            
            var paragraph = new Paragraph();

            foreach (var move in CurrentGame.PlayedMoves)
            {
                paragraph.Inlines.Add(string.Format("{0}\n", move.PrintFormat));
            }

            document.Blocks.Add(paragraph);
            
            var paginator = new PrintResultPaginator(document, CurrentGame);

            dialog.PrintDocument(paginator, "Peli tuloksen tulostus");
        }

        /// <summary>
        /// Called when [save result].
        /// </summary>
        private void OnSaveResult()
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = @"Text file|*.txt",
                InitialDirectory = Application.StartupPath
            };

             var result = saveFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                GameFileHandler.SaveGameResult(CurrentGame, saveFileDialog.FileName);
            }
        }

        /// <summary>
        /// Determines whether this instance [can save result].
        /// </summary>
        /// <returns></returns>
        private bool CanSaveResult()
        {
            return CurrentGame != null && CurrentGame.Winner != null;
        }

        /// <summary>
        /// Determines whether current game is eligiable to print result.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns></returns>
        private bool CanPrintResult(UIElement arg)
        {
            return CurrentGame != null && CurrentGame.Winner != null;
        }

        /// <summary>
        /// Called when [save game].
        /// </summary>
        private void OnSaveGame()
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = @"Gamesave file|*.xml",
                InitialDirectory = Application.StartupPath
            };

            var result = saveFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                GameFileHandler.SaveGame(CurrentGame, saveFileDialog.FileName);
            }
        }

        /// <summary>
        /// Determines whether this instance [can save game].
        /// </summary>
        /// <returns></returns>
        private bool CanSaveGame()
        {
            return CurrentGame != null;
        }

        /// <summary>
        /// Called when [load game].
        /// </summary>
        private void OnLoadGame()
        {
            var openDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = @"Gamesave file|*.xml",
                InitialDirectory = Application.StartupPath
            };

            var result = openDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var game = GameFileHandler.LoadGame(openDialog.FileName);
                if (game != null)
                {
                    CurrentGame = game;
                }
            }
        }

        /// <summary>
        /// Called when [new game].
        /// </summary>
        private void OnNewGame()
        {
            // kysy nimet
            NewGameWindow view = new NewGameWindow();
            var result = view.ShowDialog();
                
            if (result.HasValue && result.Value)
            {
                var vm = view.DataContext as NewGameViewModel;

                if (vm != null)
                {
                      var playerWhite = new Player
                    {
                        Name = vm.PlayerOne,
                        PlayerType = vm.IsPlayerOneComputer ? PlayerType.Computer : PlayerType.Human,
                        MarkerSymbol = MarkerSymbol.Ellipse,
                        Side = PlayerSide.WhiteSide
                        
                    };

                    var playerBlack = new Player
                    {
                        Name = vm.PlayerTwo,
                        PlayerType = vm.IsPlayerTwoComputer ? PlayerType.Computer : PlayerType.Human,
                        MarkerSymbol = MarkerSymbol.Ellipse,
                        Side = PlayerSide.BlackSide
                    };

                    InitGame(playerWhite, playerBlack);

                    if (CurrentGame != null)
                    {
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
                if (vm != null)
                {
                    vm.Settings = CurrentGameSettings;
                    var result = view.ShowDialog();

                    if (result.HasValue && result.Value)
                    {
                        CurrentGameSettings = vm.Settings;
                        GameFileHandler.WriteSettings(vm.Settings);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the game.
        /// </summary>
        /// <param name="white">The white.</param>
        /// <param name="black">The black.</param>
        private void InitGame(Player white, Player black)
        {
            if (CurrentGameSettings == null)
            {
                // Oletus asetuksilla
                CurrentGameSettings = GameSetting.Default;
            }

            white.SymbolColor = CurrentGameSettings.PlayerOneColor;
            black.SymbolColor = CurrentGameSettings.PlayerTwoColor;
            white.MarkerSymbol = CurrentGameSettings.PlayerOneSymbol;
            black.MarkerSymbol = CurrentGameSettings.PlayerTwoSymbol;

            if (string.IsNullOrEmpty(white.Name))
            {
                white.Name = "Player 1";
            }

            if (string.IsNullOrEmpty(black.Name))
            {
                black.Name = "Player 2";
            }

            if (CurrentGameSettings.TypeOfGame == GameType.BreakThrough)
            {
                CurrentGame = new BreakthroughGame
                {
                    Size = CurrentGameSettings.Size,
                    Setting = CurrentGameSettings,
                    PlayerBlack = black,
                    PlayerWhite = white
                };
            }
            else
            {
                CurrentGame = new CheckerGame
                {
                    Size = CurrentGameSettings.Size,
                    Setting = CurrentGameSettings,
                    PlayerBlack = black,
                    PlayerWhite = white
                };
            }

            CurrentGame.PropertyChanged += CurrentGameOnPropertyChanged;
        }

        private void CurrentGameOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Turn")
            {
                SetWait(1000);
            }
        }

        private void SetWait(int milliSeconds)
        {
            var tempTask = new Task
            (() =>
            {
                Thread.Sleep(milliSeconds);
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => AiMove(CurrentGame.CurrentPlayer)));
            }, TaskCreationOptions.LongRunning);

            tempTask.Start();
        }


        /// <summary>
        /// Called when [undo].
        /// </summary>
        private void OnUndo()
        {
            if (CurrentGame != null)
            {
                CurrentGame.Undo();
            }
        }

        /// <summary>
        /// Called when [redo].
        /// </summary>
        private void OnRedo()
        {
            if (CurrentGame != null)
            {
                CurrentGame.Redo();
            }
        }


        /// <summary>
        /// Determines whether current game can undo.
        /// </summary>
        /// <returns></returns>
        private bool CanUndo()
        {
            return CurrentGame != null && CurrentGame.PlayedMoves != null && CurrentGame.PlayedMoves.Count > 0;
        }

        /// <summary>
        /// Determines whether current game can redo.
        /// </summary>
        /// <returns></returns>
        private bool CanRedo()
        {
            return CurrentGame != null && CurrentGame.PlayedMoves != null;
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

        /// <summary>
        /// Gets the save game command.
        /// </summary>
        /// <value>
        /// The save game command.
        /// </value>
        public ICommand SaveGameCommand { get; private set; }

        /// <summary>
        /// Gets the load game command.
        /// </summary>
        /// <value>
        /// The load game command.
        /// </value>
        public ICommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Gets the save result command.
        /// </summary>
        /// <value>
        /// The save result command.
        /// </value>
        public ICommand SaveResultCommand { get; private set; }

        /// <summary>
        /// Gets the print result command.
        /// </summary>
        /// <value>
        /// The print result command.
        /// </value>
        public RelayCommand<UIElement> PrintResultCommand { get; private set; }

        /// <summary>
        /// Gets the about command.
        /// </summary>
        /// <value>
        /// The about command.
        /// </value>
        public RelayCommand AboutCommand { get; private set; }

        #endregion

        #region Ai

        

        private object AiMove(Player currentPlayer)
        {
            if (currentPlayer.PlayerType == PlayerType.Computer && _currentGame.Winner == null)
            {
                var merkit = CurrentGame.BoardItems.Where(c => c.Side == currentPlayer.Side).ToList();

                for (int i = 0; i < merkit.Count(); i++)
                {
                    var moves = CurrentGame.GetPossibleMoves(merkit[i]);

                    if (moves != null && moves.Count > 0)
                    {
                        Random rnd = new Random();
                        
                        var valittu = moves[rnd.Next(0, moves.Count() - 1)];
                        CurrentGame.Move(merkit[i], valittu);
                        break;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}