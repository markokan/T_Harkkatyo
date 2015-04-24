using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Samuxi.WPF.Harjoitus.Model;

namespace Samuxi.WPF.Harjoitus.ViewModel
{
    /// <summary>
    /// Settings view model.
    /// </summary>
    public class SettingsViewModel : ViewModelBase
    {
        private GameSetting _settings;
        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public GameSetting Settings
        {
            get {  return _settings; }
            set
            {
                _settings = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<GameType> _gameTypes;
        /// <summary>
        /// Gets or sets the game types.
        /// </summary>
        /// <value>
        /// The game types.
        /// </value>
        public ObservableCollection<GameType> GameTypes
        {
            get { return _gameTypes; }
            set
            {
                _gameTypes = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<GameSize> _gameSizes;
        /// <summary>
        /// Gets or sets the game sizes.
        /// </summary>
        /// <value>
        /// The game sizes.
        /// </value>
        public ObservableCollection<GameSize> GameSizes
        {
            get { return _gameSizes;}
            set
            {
                _gameSizes = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<MarkerSymbol> _symbols;
        /// <summary>
        /// Gets or sets the symbols.
        /// </summary>
        /// <value>
        /// The symbols.
        /// </value>
        public ObservableCollection<MarkerSymbol> Symbols
        {
            get { return _symbols;}
            set
            {
                _symbols = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel()
        {
            SelectColorCommand = new RelayCommand<int>(OnColorSelect);
            GameTypes = new ObservableCollection<GameType> {GameType.BreakThrough, GameType.Checker};
            GameSizes = new ObservableCollection<GameSize>();
            Symbols = new ObservableCollection<MarkerSymbol> { MarkerSymbol.Ellipse, MarkerSymbol.Cubic, MarkerSymbol.Triangle };

            for (int x = 8; x <= 16; x++)
            {
                for (int y = 8; y <= 16; y++)
                {
                    GameSizes.Add(new GameSize {Columns = x, Rows = y});
                }
            }

            if (Settings == null)
            {
                Settings = GameSetting.Default;
            }
        }

        /// <summary>
        /// Called when [color select].
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnColorSelect(int obj)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                string strColor = System.Drawing.ColorTranslator.ToHtml(colorDialog.Color);

                var convertFromString = ColorConverter.ConvertFromString(strColor);
                if (convertFromString != null)
                {
                    var selectedColor = (Color) convertFromString;

                    switch (obj)
                    {
                        case 0:
                            Settings.GridColorOne = new SolidColorBrush(selectedColor);
                            break;
                        case 1:
                            Settings.GridColorTwo = new SolidColorBrush(selectedColor);
                            break;
                        case 2: 
                            Settings.PlayerOneColor = selectedColor;
                            break;
                        case 3:
                            Settings.PlayerTwoColor = selectedColor;
                            break;
                    }
                }
            }
        }

        #region Commands

        /// <summary>
        /// Gets the select color command.
        /// </summary>
        /// <value>
        /// The select color command.
        /// </value>
        public RelayCommand<int> SelectColorCommand { get; private set; }
      
        #endregion
    }
}
