using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Controls
{
    /// <summary>
    /// GridUtility handle Attachment propertys related to GameBoard Grid.
    /// </summary>
    public class GridUtility
    {
        /// <summary>
        /// The row count property
        /// </summary>
        public static readonly DependencyProperty RowCountProperty = DependencyProperty.RegisterAttached("RowCount", typeof(int), typeof(GridUtility),new PropertyMetadata(-1, RowCountChanged));
        
        
        /// <summary>
        /// The column count property
        /// </summary>
        public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.RegisterAttached("ColumnCount", typeof(int), typeof(GridUtility), new PropertyMetadata(-1, ColumnCountChanged));

        /// <summary>
        /// Rows the count changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void RowCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Grid gridObject = GetGrid(obj);

            if (gridObject == null || (int) e.NewValue < 0)
            {
                return;
            }

            gridObject.RowDefinitions.Clear();

            for (int i = 0; i < (int) e.NewValue; i++)
            {
                gridObject.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1) });
            }

            
        }

    

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>count</returns>
        public static int GetRowCount(DependencyObject obj)
        {
            return (int)obj.GetValue(RowCountProperty);
        }


        /// <summary>
        /// Sets the row count.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetRowCount(DependencyObject obj, int value)
        {
            obj.SetValue(RowCountProperty, value);
        }

        /// <summary>
        /// Gets the column count.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>count</returns>
        public static int GetColumnCount(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnCountProperty);
        }

        /// <summary>
        /// Sets the column count.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetColumnCount(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnCountProperty, value);
        }


        /// <summary>
        /// Columns the count changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void ColumnCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Grid gridObject = GetGrid(obj);

            if (gridObject == null || (int) e.NewValue < 0)
            {
                return;
            }

            gridObject.ColumnDefinitions.Clear();

            for (int i = 0; i < (int) e.NewValue; i++)
            {
                gridObject.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1) });
            }
        }

        /// <summary>
        /// Gets the grid object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Grid object</returns>
        private static Grid GetGrid(DependencyObject obj)
        {
            var gridObject = obj as Grid;
            return gridObject;
        }
    }
}
