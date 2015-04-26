using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Practices.ServiceLocation;
using Samuxi.WPF.Harjoitus.Model;
using Samuxi.WPF.Harjoitus.Print;
using Samuxi.WPF.Harjoitus.Utils;
using Samuxi.WPF.Harjoitus.Views;
using Application = System.Windows.Forms.Application;


namespace Samuxi.WPF.Harjoitus.ViewModel
{
    /// @version 26.4.2015
    /// @author Marko Kangas
    /// 
    /// <summary>
    /// Main window ViewModel.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The help file location
        /// </summary>
        private const string HELP_FILE_LOCATION = @"Help\Help.html";

        /// <summary>
        /// Event property name
        /// </summary>
        private const string EVENT_PROPERTYNAME = "Turn";

        /// <summary>
        /// The winner sound event propertyname
        /// </summary>
        private const string WINNER_SOUND_EVENT_PROPERTYNAME = "Winner";

        /// <summary>
        /// The MediaPlayer
        /// </summary>
        private MediaPlayer _player = new MediaPlayer();

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

        private Cursor _mainCursor;

        /// <summary>
        /// Gets or sets the main cursor.
        /// </summary>
        /// <value>
        /// The main cursor.
        /// </value>
        public Cursor MainCursor
        {
            get { return _mainCursor; }
            set
            {
                _mainCursor = value;
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
            MainCursor = Cursors.Hand;
            RedoCommand = new RelayCommand(OnRedo, CanRedo);
            UndoCommand = new RelayCommand(OnUndo, CanUndo);
            OpenSettingsCommand = new RelayCommand(OnSettings);
            NewGameCommand = new RelayCommand(OnNewGame);
            SaveGameCommand = new RelayCommand(OnSaveGame, CanSaveGame);
            LoadGameCommand = new RelayCommand(OnLoadGame);
            SaveResultCommand = new RelayCommand(OnSaveResult, CanSaveResult);
            PrintResultCommand = new RelayCommand<UIElement>(OnPrintResult, CanPrintResult);
            AboutCommand = new RelayCommand(OnOpenAbout);
            ReplayGameCommand = new RelayCommand(OnReplayGame, CanReplayGame);
            OpenHelpCommand = new RelayCommand(OnOpenHelpCommand);

            CurrentGameSettings = GameFileHandler.ReadSetting();
            if (!CurrentGameSettings.IsEngChecked && !CurrentGameSettings.IsFinChecked)
            {
                CurrentGameSettings.IsEngChecked = true; // default
            }

            SetUiLanguage();

            // Set winner game sound
            _player.Open(new Uri(Application.StartupPath +  @"\Resources\DING1.mp3"));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when [open help command is executed].
        /// </summary>
        private void OnOpenHelpCommand()
        {
            if (System.IO.File.Exists(Application.StartupPath + "\\" + HELP_FILE_LOCATION))
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\" + HELP_FILE_LOCATION);
            }
        }

        /// <summary>
        /// Determines whether this instance [can replay game].
        /// </summary>
        /// <returns>Is replay possible</returns>
        private bool CanReplayGame()
        {
            return CurrentGame != null && CurrentGame.PlayedMoves != null && CurrentGame.PlayedMoves.Count > 0 &&
                   !CurrentGame.IsReplayRunning;
        }

        /// <summary>
        /// Called when [replay game].
        /// </summary>
        private void OnReplayGame()
        {
            CurrentGame.Replay();
        }

        /// <summary>
        /// Called when [open about].
        /// </summary>
        private void OnOpenAbout()
        {
            var aboutViewModel = ServiceLocator.Current.GetInstance<AboutViewModel>();
            if (aboutViewModel != null)
            {
                AboutWindow view = new AboutWindow();
                var result = view.ShowDialog();
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

            dialog.PrintDocument(paginator, Properties.Resources.GameResultPrinting);
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
                        // loading this is necessary to reset board...
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
                        SetUiLanguage();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the UI language.
        /// </summary>
        private void SetUiLanguage()
        {
            if (CurrentGameSettings == null || CurrentGameSettings.IsEngChecked)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-EN");
            }
            else if (CurrentGameSettings.IsFinChecked)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");
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
                // Default 
                CurrentGameSettings = GameSetting.Default;
            }

            white.SymbolColor = CurrentGameSettings.PlayerOneColor;
            black.SymbolColor = CurrentGameSettings.PlayerTwoColor;
            white.MarkerSymbol = CurrentGameSettings.PlayerOneSymbol;
            black.MarkerSymbol = CurrentGameSettings.PlayerTwoSymbol;

            if (string.IsNullOrEmpty(white.Name))
            {
                white.Name = Properties.Resources.TextPlayer1;
            }

            if (string.IsNullOrEmpty(black.Name))
            {
                black.Name = Properties.Resources.TextPlayer2;
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

            Random randomGetStarter = new Random();
            int starter = randomGetStarter.Next(1, 100);

            CurrentGame.Turn = starter % 2 == 1 ? PlayerSide.BlackSide : PlayerSide.WhiteSide;
        }

        /// <summary>
        /// Currents the game on property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void CurrentGameOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == EVENT_PROPERTYNAME)
            {
                AnimateAiMoves(1000);
            }

            if (args.PropertyName == WINNER_SOUND_EVENT_PROPERTYNAME && CurrentGame.IsGameEnd)
            {
                _player.Play();
                _player.Position = TimeSpan.FromSeconds(0);
            }
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
            return CurrentGame != null && CurrentGame.PlayedMoves != null && CurrentGame.PlayedMoves.Count > 0 &&
                   !CurrentGame.IsGameEnd;
        }

        /// <summary>
        /// Determines whether current game can redo.
        /// </summary>
        /// <returns></returns>
        private bool CanRedo()
        {
            return CurrentGame != null && CurrentGame.PlayedMoves != null &&
                   !CurrentGame.IsGameEnd;
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

        /// <summary>
        /// Gets the replay game command.
        /// </summary>
        /// <value>
        /// The replay game command.
        /// </value>
        public RelayCommand ReplayGameCommand { get; private set; }

        /// <summary>
        /// Gets the open help command.
        /// </summary>
        /// <value>
        /// The open help command.
        /// </value>
        public RelayCommand OpenHelpCommand { get; private set; }

        #endregion

        #region Ai
        /// <summary>
        /// Animates the AI moves.
        /// </summary>
        /// <param name="milliSeconds">The milliseconds.</param>
        private void AnimateAiMoves(int milliSeconds)
        {
            if (CurrentGame != null && !CurrentGame.IsReplayRunning)
            {
                var tempTask = new Task
                    (() =>
                    {
                        Thread.Sleep(milliSeconds);
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(
                            new Action(() => AiMove(CurrentGame.CurrentPlayer)));
                    }, TaskCreationOptions.LongRunning);

                tempTask.Start();
            }
        }


        /// <summary>
        /// Ais the move.
        /// </summary>
        /// <param name="currentPlayer">The current player.</param>
        /// <returns></returns>
        private object AiMove(Player currentPlayer)
        {
            Random aimoveRandom = new Random();

            if (currentPlayer.PlayerType == PlayerType.Computer && !CurrentGame.IsGameEnd)
            {
                var merkit = CurrentGame.BoardItems.Where(c => c.Side == currentPlayer.Side).ToList();

                var moves = CurrentGame.GetAllPossibleMoves(CurrentGame.Turn);

                if (moves != null && moves.Count > 0)
                {
                    var choosedMove = moves[aimoveRandom.Next(0, moves.Count())];

                    var boardItemToMove = merkit.FirstOrDefault(c => c.Id == choosedMove.Id);
                    CurrentGame.Move(boardItemToMove, choosedMove.Position);
                    
                }
            }

            return false;
        }

        #endregion
    }
}