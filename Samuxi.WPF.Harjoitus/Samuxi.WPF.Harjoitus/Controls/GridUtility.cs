using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Samuxi.WPF.Harjoitus.Controls
{
    public class GridUtility
    {
        public static readonly DependencyProperty RowCountProperty = DependencyProperty.RegisterAttached("RowCount", typeof(int), typeof(GridUtility),new PropertyMetadata(-1, RowCountChanged));
        public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.RegisterAttached("ColumnCount", typeof(int), typeof(GridUtility), new PropertyMetadata(-1, ColumnCountChanged));

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
        // Get
        public static int GetRowCount(DependencyObject obj)
        {
            return (int)obj.GetValue(RowCountProperty);
        }

        // Set
        public static void SetRowCount(DependencyObject obj, int value)
        {
            obj.SetValue(RowCountProperty, value);
        }

        public static int GetColumnCount(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnCountProperty);
        }

        public static void SetColumnCount(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnCountProperty, value);
        }
     

        public static void ColumnCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Grid gridObject = GetGrid(obj);

            if (gridObject == null || (int)e.NewValue < 0)

            gridObject.ColumnDefinitions.Clear();

            for (int i = 0; i < (int) e.NewValue; i++)
            {
                gridObject.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1) });
            }
        }

        private static Grid GetGrid(DependencyObject obj)
        {
            var gridObject = obj as Grid;
            return gridObject;
        }
    }
}
