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
        /// <summary>
        /// The Start point to drag
        /// </summary>
        private Point _startPointToDrag;

        /// <summary>
        /// Gets or sets the game.
        /// </summary>
        /// <value>
        /// The game.
        /// </value>
        public IGame Game
        {
            get { return (IGame)GetValue(GameDependencyPropertyProperty); }
            set { SetValue(GameDependencyPropertyProperty, value); }
        }

        /// <summary>
        /// The game dependency property property
        /// </summary>
        public static readonly DependencyProperty GameDependencyPropertyProperty =
            DependencyProperty.Register("Game", typeof(IGame), typeof(GameBoardControl), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="GameBoardControl"/> class.
        /// </summary>
        public GameBoardControl()
        {
            InitializeComponent();
        }

        #region Adorner
        /// <summary>
        /// Creates the drag adorner.
        /// </summary>
        /// <param name="sender">The sender.</param>
        static void CreateDragAdorner(PlayMarker sender)
        {
            if (sender != null)
            {
                DataTemplate template = PlayMarker.GetDragAdornerTemplate(sender);

                if (template != null)
                {
                    UIElement rootElement = (UIElement)Application.Current.MainWindow.Content;

                    ContentPresenter contentPresenter = new ContentPresenter
                    {
                        Content = sender.Item.FillBrush,
                        ContentTemplate = template
                    };

                    UIElement adornment = contentPresenter;
                    
                    DragAdorner = new DragAdorner(rootElement, adornment);
                }
            }
        }

        private static DragAdorner _dragAdorner;
        /// <summary>
        /// Gets or sets the drag adorner.
        /// </summary>
        /// <value>
        /// The drag adorner.
        /// </value>
        static DragAdorner DragAdorner
        {
            get { return _dragAdorner; }
            set
            {
                if (_dragAdorner != null)
                {
                    _dragAdorner.Detatch();
                }

                _dragAdorner = value;
            }
        }

        #endregion

        #region Drag And Drop

        private static PlayMarker _currentUiElement;

        /// <summary>
        /// Handles the OnPreviewMouseLeftButtonDown event of the MarkerControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void MarkerControl_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPointToDrag = e.GetPosition(null);

            if (DragAdorner == null)
            {
                CreateDragAdorner(_currentUiElement);
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the ItemsContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
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

                _currentUiElement = marker;

                if (senderControl != null && marker != null)
                {
                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("BoardItem", marker.Item);
                    DragDrop.DoDragDrop(marker, dragData, DragDropEffects.Move);
                }

                if (DragAdorner != null)
                {
                    DragAdorner.MousePosition = e.GetPosition(DragAdorner.AdornedElement);
                    DragAdorner.InvalidateVisual();
                }
            } 
        }

        /// <summary>
        /// Handles the Drop event of the ItemsControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void ItemsControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Handled == false)
            {
                var board = FindAnchestor<Grid>((DependencyObject)e.OriginalSource);
                if (board != null && e.Effects == DragDropEffects.Move)
                {
                    var positionOnBoard = e.GetPosition(board);
                    
                    var boardItem = (BoardItem) e.Data.GetData("BoardItem");

                    if (boardItem != null)
                    {
                        GamePosition position = new GamePosition
                        {
                            Column = (int)positionOnBoard.X,
                            Row =  (int)positionOnBoard.Y
                        };
                        
                        Game.Move(boardItem, position);

                        if (Game.Winner != null)
                        {
                            boardItem.Symbol = MarkerSymbol.Winner;
                        }

                    }
                }
            }

            DragAdorner = null;
            _currentUiElement = null;
        }

        /// <summary>
        /// Handles the OnDragEnter event of the ItemsControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void ItemsControl_OnDragEnter(object sender, DragEventArgs e)
        {
        }

        /// <summary>
        /// Finds the anchestor DependencyObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current">The current.</param>
        /// <returns>DependencyObject</returns>
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

        /// <summary>
        /// Handles the OnDragOver event of the BoardItemsControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
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

        private void BoardItemsControl_OnPreviewDragOver(object sender, DragEventArgs e)
        {
            if (DragAdorner == null)
            {
                CreateDragAdorner(_currentUiElement);
            }

            if (DragAdorner != null)
            {
                DragAdorner.Visibility = Visibility.Visible;
                DragAdorner.MousePosition = e.GetPosition(DragAdorner.AdornedElement);
                DragAdorner.InvalidateVisual();
            }
        }

        private void BoardItemsControl_OnPreviewDragLeave(object sender, DragEventArgs e)
        {
            if (DragAdorner != null)
            {
                DragAdorner.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        private void BoardItemsControl_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition((UIElement)sender);

            // Perform the hit test against a given portion of the visual object tree.
            HitTestResult result = VisualTreeHelper.HitTest((UIElement)sender, pt);

            if (result != null)
            {
                var playMarker = GetVisualParent<PlayMarker>(result.VisualHit);

                if (playMarker != null && (Game.Turn != playMarker.Item.Side || Game.Winner != null))
                {
                    if (playMarker.Item.Side != PlayerSide.None)
                    {
                        if (Game.IsValidMovement(playMarker.Item))
                        {
                            var choosedItem = Game.BoardItems.FirstOrDefault(c => c.IsSelected);
                            if (choosedItem != null)
                            {
                                choosedItem.IsSelected = false;

                                if (choosedItem.Id != playMarker.Item.Id)
                                {
                                    playMarker.Item.IsSelected = true;
                                }
                            }
                            else
                            {
                                playMarker.Item.IsSelected = true;
                            }
                        }
                    }
                    else
                    {
                        if (playMarker.Item.IsPossibleMove)
                        {
                            var choosedItem = Game.BoardItems.FirstOrDefault(c => c.IsSelected);
                            if (choosedItem != null)
                            {
                                Game.Move(choosedItem, playMarker.Item.GamePosition);
                            }
                        }
                    }
                }
            }
        }

        public static T GetVisualParent<T>(DependencyObject element) where T : DependencyObject
        {
            while (element != null && !(element is T))
                element = VisualTreeHelper.GetParent(element);

            return (T)element;
        }
    }
}
