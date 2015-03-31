using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Samuxi.WPF.Harjoitus.Model;

namespace Samuxi.WPF.Harjoitus.Controls
{
    /// <summary>
    /// Interaction logic for PlayMarker.xaml
    /// </summary>
    public partial class PlayMarker : UserControl
    {
        public BoardItem Item
        {
            get { return (BoardItem)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("Item", typeof(BoardItem), typeof(PlayMarker), new PropertyMetadata(null));
    
        public PlayMarker()
        {
            InitializeComponent();
        }
    }
}
