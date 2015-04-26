using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Samuxi.WPF.Harjoitus.Model;

namespace Samuxi.WPF.Harjoitus.Controls
{
    /// <summary>
    /// Interaction logic for PlayMarker.xaml
    /// </summary>
    public partial class PlayMarker : UserControl
    {
        private const double DEFAULT_SYMBOL_SIZE = 0.5d;

        /// <summary>
        /// Gets or sets the Boarditem.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public BoardItem Item
        {
            get { return (BoardItem)GetValue(BoardItemProperty); }
            set { SetValue(BoardItemProperty, value); }
        }

        /// <summary>
        /// The board item property
        /// </summary>
        public static readonly DependencyProperty BoardItemProperty = DependencyProperty.Register("Item", typeof(BoardItem), typeof(PlayMarker), 
            new PropertyMetadata(null, PropertyChangedCallback));

        /// <summary>
        /// Properties the changed callback.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var maker = dependencyObject as PlayMarker;
            if (maker != null)
            {
                
                maker.Item.PropertyChanged += (sender, args) =>
                {
                    System.Diagnostics.Debug.WriteLine("Playmaker: " + args.PropertyName);
                    if ((args.PropertyName == "Row" || args.PropertyName == "Column") && maker.Item.Side != PlayerSide.None)
                    {
                        maker.RaiseMoveEvent();
                    }

                    if ((args.PropertyName == "IsKing"))
                    {
                        maker.RaiseToKingEvent();
                        maker.CurrentContentControl.Width = 1d;
                        maker.CurrentContentControl.Height = 1d;
                    }
                }; 
            }
        }

        public double SymbolWidth
        {
            get { return (double)GetValue(SymbolWidthProperty); }
            set { SetValue(SymbolWidthProperty, value); }
        }

        public static readonly DependencyProperty SymbolWidthProperty = DependencyProperty.Register("SymbolWidth", typeof(double), typeof(PlayMarker),
            new PropertyMetadata(DEFAULT_SYMBOL_SIZE, PropertyChangedCallback));


        public double SymbolHeight
        {
            get { return (double)GetValue(SymbolHeightProperty); }
            set { SetValue(SymbolHeightProperty, value); }
        }

        public static readonly DependencyProperty SymbolHeightProperty = DependencyProperty.Register("SymbolHeight", typeof(double), typeof(PlayMarker),
            new PropertyMetadata(DEFAULT_SYMBOL_SIZE, PropertyChangedCallback));

        /// <summary>
        /// The drag adorner template property
        /// </summary>
        public static readonly DependencyProperty DragAdornerTemplateProperty =
          DependencyProperty.RegisterAttached("DragAdornerDataTemplate", typeof(DataTemplate), typeof(PlayMarker));

        /// <summary>
        /// Gets the drag adorner template.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static DataTemplate GetDragAdornerTemplate(UIElement target)
        {
            return (DataTemplate)target.GetValue(DragAdornerTemplateProperty);
        }

        /// <summary>
        /// Sets the drag adorner template.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        public static void SetDragAdornerTemplate(UIElement target, DataTemplate value)
        {
            target.SetValue(DragAdornerTemplateProperty, value);
        }


        /// <summary>
        /// The move event
        /// </summary>
        public static readonly RoutedEvent MoveEvent = EventManager.RegisterRoutedEvent("Move", RoutingStrategy.Bubble, typeof(RoutedEventHandler), 
            typeof(PlayMarker));


     
        /// <summary>
        /// Occurs when [move].
        /// </summary>
        public event RoutedEventHandler Move
        {
            add { AddHandler(MoveEvent, value); }
            remove { RemoveHandler(MoveEvent, value); }
        }

        /// <summary>
        /// Raises the move event.
        /// </summary>
        void RaiseMoveEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(MoveEvent);
            RaiseEvent(newEventArgs);
        }

        public static readonly RoutedEvent SymbolToKingEvent = EventManager.RegisterRoutedEvent("SymbolToKing", RoutingStrategy.Bubble, typeof(RoutedEventHandler),
         typeof(PlayMarker));

        public event RoutedEventHandler SymbolToKing
        {
            add { AddHandler(SymbolToKingEvent, value); }
            remove { RemoveHandler(SymbolToKingEvent, value); }
        }

        void RaiseToKingEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SymbolToKingEvent);
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayMarker"/> class.
        /// </summary>
        public PlayMarker()
        {
            InitializeComponent();
        }
    }
}
