using GalaSoft.MvvmLight;
using Samuxi.WPF.Harjoitus.Utils;

namespace Samuxi.WPF.Harjoitus.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        private string _version;

        /// <summary>
        /// Gets the game version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version
        {
            get { return _version;}
            private set
            {
                _version = value;
                RaisePropertyChanged();
            }
        }

        public AboutViewModel()
        {
            Version = ProgramInfoUtil.Version;
        }
    }
}
