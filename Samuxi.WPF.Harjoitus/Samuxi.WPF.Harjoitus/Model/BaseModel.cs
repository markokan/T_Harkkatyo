using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// @version 26.4.2015
    /// @author Marko Kangas
    /// 
    /// <summary>
    /// Base model to objects.
    /// </summary>
    public abstract class BaseModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (null != PropertyChanged)
            {
                if (Dispatcher.CurrentDispatcher.CheckAccess())
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                else
                {
                    Dispatcher.CurrentDispatcher.BeginInvoke(PropertyChanged, this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}
