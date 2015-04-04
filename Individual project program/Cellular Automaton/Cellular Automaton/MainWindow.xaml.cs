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
using MahApps.Metro.Controls;

namespace Cellular_Automaton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        static Brush CreateGridBrush(double actualHeight,double actualWidth, Size tileSize, double gridThicknessMultiplier)
        {
            var gridColor = Brushes.Black;
            var gridThickness = 1.0*gridThicknessMultiplier;
            var tileRect = new Rect(tileSize);
            
            var gridTile = new DrawingBrush
            {
                Stretch = Stretch.None,
                TileMode = TileMode.Tile,
                Viewport = tileRect,
                ViewportUnits = BrushMappingMode.Absolute,
                Drawing = new GeometryDrawing
                {
                    Pen = new Pen(gridColor, gridThickness),
                    Geometry = new GeometryGroup
                    {
                        Children = new GeometryCollection
                {
                    new LineGeometry(tileRect.TopLeft,tileRect.TopRight ),
                    new LineGeometry(tileRect.TopLeft, tileRect.BottomLeft)
                }
                    }
                }
            };
            Rect canvas = new Rect(new Point(-actualWidth,-actualHeight),new Point(actualWidth,actualHeight));
            var offsetGrid = new DrawingBrush
            {
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Center,
                AlignmentY = AlignmentY.Center,
                Transform = new TranslateTransform(0, 0),
                Drawing = new GeometryDrawing
                {
                    Geometry = new RectangleGeometry(canvas),
                    Brush = gridTile
                }
            };

            return offsetGrid;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Brush brush = CreateGridBrush(gridCanvas.ActualWidth, gridCanvas.ActualHeight, new Size(10, 10),1);
            gridCanvas.Background = brush;
        }

        private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)zoomSlider.Value;
            Brush brush = CreateGridBrush(gridCanvas.ActualHeight,gridCanvas.ActualWidth, new Size(value, value),(double)value/10);
            gridCanvas.Background = brush;
        }
    }
}
