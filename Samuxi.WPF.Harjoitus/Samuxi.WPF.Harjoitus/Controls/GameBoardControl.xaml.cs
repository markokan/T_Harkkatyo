using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for GameBoardControl.xaml
    /// </summary>
    public partial class GameBoardControl : UserControl
    {
        private Point _startPointToDrag;

        public IGame Game
        {
            get { return (IGame)GetValue(GameDependencyPropertyProperty); }
            set { SetValue(GameDependencyPropertyProperty, value); }
        }
        public static readonly DependencyProperty GameDependencyPropertyProperty =
            DependencyProperty.Register("Game", typeof(IGame), typeof(GameBoardControl), new PropertyMetadata(null, 
                PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            
        }


        public GameBoardControl()
        {
            InitializeComponent();
        }

        #region Drag And Drop



        #endregion

        private void PawnMarkerControl_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPointToDrag = e.GetPosition(null);
        }


        private void ItemsContainer_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPointToDrag - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed && (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                
                var senderControl = sender as ItemsControl;
                PlayMarker marker = FindAnchestor<PlayMarker>((DependencyObject)e.OriginalSource);

                if (senderControl != null && marker != null)
                {
                    //BoardItem boardItem = (BoardItem) senderControl.ItemContainerGenerator.ItemFromContainer(marker);

                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("BoardItem", marker.Item);
                    DragDrop.DoDragDrop(marker, dragData, DragDropEffects.Move);
                }
            } 
        }

        private void ItemsControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Handled == false)
            {
                var board = FindAnchestor<Grid>((DependencyObject)e.OriginalSource);
                if (board != null && e.Effects == DragDropEffects.Move)
                {
                    var diu = e.GetPosition(board);
                    
                    var boardItem = (BoardItem) e.Data.GetData("BoardItem");

                    if (boardItem != null)
                    {
                        GamePosition position = new GamePosition
                        {
                            Column = (int)diu.X,
                            Row =  (int)diu.Y
                        };
                        
                        Game.Move(boardItem, position);
                        if (Game.Winner != null)
                            boardItem.Symbol = MarkerSymbol.Winner;
                        
                    }
                }
            }
        }

        private void ItemsControl_OnDragEnter(object sender, DragEventArgs e)
        {
         

        }

        private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                var findAnchestor = current as T;
                if (findAnchestor != null)
                {
                    return findAnchestor;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void BoardItemsControl_OnDragOver(object sender, DragEventArgs e)
        {
            var board = FindAnchestor<Grid>((DependencyObject)e.OriginalSource);

            if (board != null)
            {
                var dragPosPoint = e.GetPosition(board);
                var boardItem = (BoardItem) e.Data.GetData("BoardItem");

                // Tarkasta säännöt
                if (Game.Turn != boardItem.Side || Game.Winner != null || !Game.IsValidMovement(boardItem, dragPosPoint))
                {
                    e.Effects = DragDropEffects.None;
                }
            }

            e.Handled = true;
        }
    }
}
