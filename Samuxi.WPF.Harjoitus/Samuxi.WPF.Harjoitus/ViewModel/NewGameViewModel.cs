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

        private bool _isPlayerOneComputer;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is player one computer type.
        /// Otherwise it is human.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is player one computer; otherwise Human, <c>false</c>.
        /// </value>
        public bool IsPlayerOneComputer
        {
            get { return _isPlayerOneComputer;}
            set
            {
                _isPlayerOneComputer = value;
                RaisePropertyChanged();
            }
        }

        private bool _isPlayerTwoComputer;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is player two computer.
        /// Otherwise it is human.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is player two computer; otherwise Human, <c>false</c>.
        /// </value>
        public bool IsPlayerTwoComputer
        {
            get { return _isPlayerTwoComputer;}
            set
            {
                _isPlayerTwoComputer = value;
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
