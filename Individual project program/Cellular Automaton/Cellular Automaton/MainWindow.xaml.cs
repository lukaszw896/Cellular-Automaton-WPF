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
        CellularAutomaton cellularAutomaton;
        int _rowNum = 100;
        int _colNum = 100;
        double gridStartWidth = 500;
        double gridStartHeight = 500;
        bool doWork = false;
        public MainWindow()
        {
            cellularAutomaton = new CellularAutomaton(_rowNum,_colNum);
            InitializeComponent();
            LifeGrid.Height = gridStartHeight;
            LifeGrid.Width = gridStartWidth;
            GridBorder.Height = gridStartHeight;
            GridBorder.Width = gridStartWidth;
            
        }
        
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitGrid();
            PopulateGrid();
            ApplyRectStyle();
            #region bitmap
            /*IList<Color> color = null;
            int stride = (30 * (4 + 40) + 4) * 4;
            WriteableBitmap bitmap = new WriteableBitmap(new BitmapImage(new Uri("C:\\Users\\Łukasz\\Desktop\\gridBackground.png")));
            double dpix = bitmap.DpiX;
            double dpiy = bitmap.DpiY;
            bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), CreateGrid(30,20), stride, 0);
            gridImage.Source = bitmap;*/
            #endregion
            //CreateGrid(100, 100);
          //  gridStartHeight = LifeGrid.Height;
           // gridStartWidth = LifeGrid.Width;
        }

        #region bitmap version
        /*
        public byte[] CreateGrid(int width, int height)
        {
            //size of one cell in pixels
             // width of bitmap (4 + 40) * N +4
            
            int stride = (width*(4+40)+4) * 4;
            int size = (height*(4+40)+4) * stride;
            byte[] pixels = new byte[size];
            int startIndex = 0;
        
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    InsertFullRow(pixels, stride, startIndex);
                    startIndex +=stride;
                }
                for (int k = 0; k < 40; k++)
                {
                    InsertCellsRow(pixels, width, startIndex);
                    startIndex += stride;
                }
            }
            for (int l = 0; l < 4; l++)
            {
                InsertFullRow(pixels, stride, startIndex);
                startIndex += stride;
            }

          
                return pixels;
        }

        public void InsertFullRow(byte[] pixels, int stride, int startIndex)
        {
            for (int i = 0; i < stride; i++)
            {
                pixels[startIndex + i] = 120;
            }
        }
        public void InsertCellsRow(byte[] pixels, int columnNumber, int startIndex)
        {
            for (int i = 0; i < columnNumber; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    pixels[startIndex] = 120;
                    startIndex++;
                }
                for (int k = 0; k < 40; k++)
                {
                    pixels[startIndex] = 255;
                    startIndex++;
                }
            }
            for (int l = 0; l < 4; l++)
            {
                pixels[startIndex] = 120;
                startIndex++;
            }
        }
        */
        #endregion


        private void InitGrid()
        {
            LifeGrid.Children.Clear();
            LifeGrid.RowDefinitions.Clear();
            LifeGrid.ColumnDefinitions.Clear();
            //adding rows
            for (int i = 0; i < _rowNum; i++)
            {
                RowDefinition rd = new RowDefinition();
                Binding rowBinding = new Binding("Height");
                rowBinding.Source = cellularAutomaton.CellGrid[0][i];
                rd.SetBinding(RowDefinition.HeightProperty, rowBinding);
                LifeGrid.RowDefinitions.Add(rd);
            }
            //adding columns
            for (int i = 0; i < _colNum; i++)
            {
                LifeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void PopulateGrid()
        {
            for (int row = 0; row < _rowNum; row++)
            {
                for (int col = 0; col <_colNum; col++)
                {
                    Rectangle rect = new Rectangle();
                    Grid.SetRow(rect, row);
                    Grid.SetColumn(rect, col);
                    LifeGrid.Children.Add(rect);
                    rect.DataContext = cellularAutomaton.CellGrid[row][col];
                    rect.MouseDown += new MouseButtonEventHandler(Rect_OnMouseDown);
                   // rect.MouseMove += new MouseEventHandler(Rect_OnMouseEnter);
                }
            }
        }

        private void Rect_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            Cell cell = (Cell)rect.DataContext;
            cell.IsAlive = true;

        }

        private void ApplyRectStyle()
        {
            Brush cellBrush = GetCellBrush();
            Style style = new Style(typeof(Rectangle), (Style)LifeGrid.FindResource(typeof(Rectangle)));
            Setter setter = new Setter();
            setter.Property = Rectangle.FillProperty;
            setter.Value = cellBrush;
            style.Setters.Add(setter);
            LifeGrid.Resources.Remove("RectStyle");
            LifeGrid.Resources.Add("RectStyle", style);
            UIElementCollection rects = LifeGrid.Children;
            foreach (UIElement uie in rects)
                ((Rectangle)uie).Style = (Style)(LifeGrid.Resources["RectStyle"]);
        }

        private Brush GetCellBrush()
        {
           
                RadialGradientBrush brush = new RadialGradientBrush();
                brush.GradientOrigin = new Point(0.5, 0.5);
                brush.RadiusX = 0.7;
                brush.RadiusY = 0.7;                
                brush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.5));
                brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
                brush.Freeze();

                return brush;
        }
        

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            int timeMS = 0;
            try
            {
                timeMS = int.Parse(timeTextBox.Text);
            }
            catch
            {
                throw new SystemException("Time have to be given in a number");
                return;
            }
            if (StartButton.Content.Equals("Start"))
            {
                doWork=true;
                StartButton.Content = "Stop";
                List<SubRule4PointNH> rule = new List<SubRule4PointNH>();
                bool[] relevantNeighbors = new bool[4] { true, true, true, true };
                rule.Add(new SubRule4PointNH(relevantNeighbors, 1));
                rule.Add(new SubRule4PointNH(relevantNeighbors, 2));
                rule.Add(new SubRule4PointNH(relevantNeighbors, 3));
                while (doWork)
                {
                    await CellularAutomaton.CalculateNewGeneration_4PNH(cellularAutomaton.CellGrid, rule,timeMS);
                }
            }
           // System.Threading.Thread.Sleep(200);
            else
            {
                doWork = false;
                StartButton.Content = "Start";
            }
        }

        private void zoomSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            double value = zoomSlider.Value;
            LifeGrid.Width = gridStartWidth * value;
            LifeGrid.Height = gridStartHeight * value;
            GridBorder.Height = LifeGrid.Height;
            GridBorder.Width = LifeGrid.Width;
        }


        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            CellularAutomaton.ClearGrid(cellularAutomaton.CellGrid);    
        }
    }
}
