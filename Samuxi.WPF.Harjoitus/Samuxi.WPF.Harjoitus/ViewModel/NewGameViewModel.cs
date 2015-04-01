using GalaSoft.MvvmLight;

namespace Samuxi.WPF.Harjoitus.ViewModel
{
    /// <summary>
    /// New game window logic.
    /// </summary>
    public class NewGameViewModel : ViewModelBase
    {
        private string _one;
        /// <summary>
        /// Gets or sets the player one name.
        /// </summary>
        /// <value>
        /// The player one.
        /// </value>
        public string PlayerOne
        {
            get { return _one; }
            set
            {
                _one = value;
                RaisePropertyChanged();
            }
        }

        private string _two;
        /// <summary>
        /// Gets or sets the player two name.
        /// </summary>
        /// <value>
        /// The player two.
        /// </value>
        public string PlayerTwo
        {
            get { return _two; }
            set
            {
                _two = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewGameViewModel"/> class.
        /// </summary>
        public NewGameViewModel()
        {
            
        }
    }
}
