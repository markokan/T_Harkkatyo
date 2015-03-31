using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using Samuxi.WPF.Harjoitus.Model;

namespace Samuxi.WPF.Harjoitus.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        private IGame _currentGame;
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
            if (IsInDesignMode)
            {
           
            }
            else
            {

                CurrentGame = new BreakThroungGame();

               
            }
        }
    }
}