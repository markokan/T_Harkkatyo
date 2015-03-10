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

namespace Samuxi.WPF.Harjoitus.Controls
{
    /// <summary>
    /// Interaction logic for GameBoardControl.xaml
    /// </summary>
    public partial class GameBoardControl : UserControl
    {
        public GameBoardControl()
        {
            InitializeComponent();
            
            DrawBoard();
        }

        private void DrawBoard()
        {

            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto});
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition{ Width = GridLength.Auto});
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto});
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var color = new SolidColorBrush(Colors.White);

                    var valinta = y % 2 == 0;

                    if (valinta && x % 2 == 0)
                        color = new SolidColorBrush(Colors.LightGray);
                    else if (!valinta && x % 2 != 0)
                        color = new SolidColorBrush(Colors.LightGray);

                    PlayMarker marker = null;
                    if (y == 0 || y == 1)
                        marker = new PlayMarker();
                    else if (8 - y == 1 || 8-y == 2)
                        marker = new PlayMarker();

                    if (marker != null)
                    {
                       
                        marker.MouseDown += MarkerOnMouseDown;
                        marker.MouseUp += delegate(object sender, MouseButtonEventArgs args)
                        {
                            ((PlayMarker) sender).Opacity = 1d;
                        };
                        
                        
                    }

                    

                    Border b = new Border
                    {
                        Width = 50,
                        Height = 50,
                        Background = color,
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(1),
                        Child = marker,
                        AllowDrop = true
                        
                    };


                    Grid.SetColumn(b, x);
                    Grid.SetRow(b, y);
                    MainGrid.Children.Add(b);
                }
            }
        }

        private void MarkerOnDragEnter(object sender, DragEventArgs dragEventArgs)
        {
            var nappi = sender as PlayMarker;
            if (nappi != null)
            {
                var currentX = Grid.GetColumn(nappi);
                var currentY = Grid.GetRow(nappi);

                foreach (var r in MainGrid.Children.OfType<Border>())
                {
                    if (r.Child == null)
                    {
                        var by = Grid.GetRow(r);
                        var bx = Grid.GetColumn(r);

                        if (currentY + 1 == by && currentX == bx)
                        {
                            r.Background = new SolidColorBrush(Colors.LightGreen);
                        }
                        else
                        {
                            
                        }
                    }
                }



            }
        }

        private void MarkerOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var nappi = sender as PlayMarker;
            if (nappi != null)
            {
                nappi.Opacity = 0.5d;
            }
        }
    }
}
