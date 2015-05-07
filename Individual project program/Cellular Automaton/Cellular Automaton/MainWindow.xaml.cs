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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Interop;

namespace Cellular_Automaton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        CellularAutomaton cellularAutomaton;
        int _rowNum = 50;
        int _colNum = 50;
        double gridStartWidth = 500;
        double gridStartHeight = 500;
        bool doWork = false;
        public static Dictionary<String, List<SubRule4PointNH>> ruleList4PNH = new Dictionary<string, List<SubRule4PointNH>>();
        public static Dictionary<String,List<SubRule8PointNH>> ruleList8PNH = new Dictionary<String, List<SubRule8PointNH>>();
        public static Dictionary<String, List<SubRule24PointNH>> ruleList24PNH = new Dictionary<string, List<SubRule24PointNH>>();

        public MainWindow()
        {
            cellularAutomaton = new CellularAutomaton(_rowNum,_colNum);
            InitializeComponent();
            LifeGrid.Height = gridStartHeight;
            LifeGrid.Width = gridStartWidth;
            GridBorder.Height = gridStartHeight;
            GridBorder.Width = gridStartWidth;
            PopulateRuleLists();
            addItemsToCheckBox(4);
            
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
            FourPNHCHeckBox.IsChecked = true;
            InitEnviromentGrid(4);
            PopulateEnviromentGrid(4);
            ruleComboBox.SelectedValue = "Weird shapes";
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

        #region POPULATING GRIDS
        private void InitGrid()
        {
            LifeGrid.Children.Clear();
            LifeGrid.RowDefinitions.Clear();
            LifeGrid.ColumnDefinitions.Clear();
            //adding rows
            for (int i = 0; i < _rowNum; i++)
            {
                RowDefinition rd = new RowDefinition();
                LifeGrid.RowDefinitions.Add(rd);
            }
            //adding columns
            for (int i = 0; i < _colNum; i++)
            {
                LifeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void InitEnviromentGrid(int neighboursCount)
        {
            int rowColNum = 0;
            if (neighboursCount == 24){ rowColNum = 5; }
            else { rowColNum = 3; }

            enviromentGrid.Children.Clear();
            enviromentGrid.RowDefinitions.Clear();
            enviromentGrid.ColumnDefinitions.Clear();
            //adding rows
            for (int i = 0; i < rowColNum; i++)
            {
                RowDefinition rd = new RowDefinition();
                enviromentGrid.RowDefinitions.Add(rd);
            }
            //adding columns
            for (int i = 0; i < rowColNum; i++)
            {
                enviromentGrid.ColumnDefinitions.Add(new ColumnDefinition());
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
        private void PopulateEnviromentGrid(int  neighboursCount)
        {
            int rowColNum = 0;
            if (neighboursCount == 24) { rowColNum = 5; }
            else { rowColNum = 3; }
            for (int row = 0; row < rowColNum; row++)
            {
                for (int col = 0; col < rowColNum; col++)
                {
                    if (!(neighboursCount == 4 && ((row == 0 && col == 0) || (row == 2 && col == 2) || (row == 0 && col == 2) || (row == 2 && col == 0))))
                    {


                        Label label = new Label();
                        label.BorderBrush = new SolidColorBrush(Colors.Black);
                        label.SetValue(BorderThicknessProperty, new Thickness(3));
                        int tmp = rowColNum / 2;
                        if (row == tmp && col == tmp)
                        {
                            label.Background = new SolidColorBrush(Colors.Gray);
                        }
                        else
                            label.Background = new SolidColorBrush(Colors.White);
                        Grid.SetRow(label, row);
                        Grid.SetColumn(label, col);
                        enviromentGrid.Children.Add(label);
                    }
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
        #endregion 

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
            }
            if (StartButton.Content.Equals("Start"))
            {
                object myLock = new object();
                doWork=true;
                StartButton.Content = "Stop";
                if ((bool)FourPNHCHeckBox.IsChecked)
                {
                    List<SubRule4PointNH> rule = ruleList4PNH[ruleComboBox.SelectedValue.ToString()];
                    while (doWork)
                    {
                        await CellularAutomaton.CalculateNewGeneration_4PNH(cellularAutomaton.CellGrid, rule, timeMS);
                    }
                }
                else if ((bool)EightPNHCHeckBox.IsChecked)
                {
                    List<SubRule8PointNH> rule = ruleList8PNH[ruleComboBox.SelectedValue.ToString()];
                    while (doWork)
                    {
                        await CellularAutomaton.CalculateNewGeneration_8PNH(cellularAutomaton.CellGrid, rule, timeMS);
                    }
                }
                else if ((bool)TwentyFivePNHCHeckBox.IsChecked)
                {
                    List<SubRule24PointNH> rule = ruleList24PNH[ruleComboBox.SelectedValue.ToString()];
                    while (doWork)
                    {
                        await CellularAutomaton.CalculateNewGeneration_24PNH(cellularAutomaton.CellGrid, rule, timeMS);
                    }
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

        private void FourPNHCHeckBox_Click(object sender, RoutedEventArgs e)
        {
            FourPNHCHeckBox.IsChecked = true;
            EightPNHCHeckBox.IsChecked = false;
            TwentyFivePNHCHeckBox.IsChecked = false;
            InitEnviromentGrid(4);
            PopulateEnviromentGrid(4);
            addItemsToCheckBox(4);
            if (ruleComboBox.Items.Count > 0)
            {
                ruleComboBox.SelectedIndex = 0;
            }
            else ruleComboBox.SelectedValue = "No rules";
            
        }

        private void EightPNHCHeckBox_Click(object sender, RoutedEventArgs e)
        {
            FourPNHCHeckBox.IsChecked = false;
            EightPNHCHeckBox.IsChecked = true;
            TwentyFivePNHCHeckBox.IsChecked = false;
            InitEnviromentGrid(8);
            PopulateEnviromentGrid(8);
            addItemsToCheckBox(8);
            if (ruleComboBox.Items.Count > 0){  ruleComboBox.SelectedIndex = 0;}
            else ruleComboBox.SelectedValue = "No rules";
        }

        private void TwentyFivePNHCHeckBox_Click(object sender, RoutedEventArgs e)
        {
            FourPNHCHeckBox.IsChecked = false;
            EightPNHCHeckBox.IsChecked = false;
            TwentyFivePNHCHeckBox.IsChecked = true;
            InitEnviromentGrid(24);
            PopulateEnviromentGrid(24);
            addItemsToCheckBox(24);
            if (ruleComboBox.Items.Count > 0)
            {
                ruleComboBox.SelectedIndex = 0;
            }
            else ruleComboBox.SelectedValue = "No rules";
        }

        private void PopulateRuleLists()
        {
            List<SubRule4PointNH> rule = new List<SubRule4PointNH>();
            bool[] relevantNeighbors = new bool[4] { true, true, true, true };
            rule.Add(new SubRule4PointNH(relevantNeighbors, 1));
            relevantNeighbors = new bool[4] { true, true, true, true };
            rule.Add(new SubRule4PointNH(relevantNeighbors, 2));
            relevantNeighbors = new bool[4] { true, true, true, true };
            rule.Add(new SubRule4PointNH(relevantNeighbors, 3));
            ruleList4PNH.Add("Romb", rule);

            rule = new List<SubRule4PointNH>();
            relevantNeighbors = new bool[4] { true, true, true, true };
            rule.Add(new SubRule4PointNH(relevantNeighbors, 2));
            relevantNeighbors = new bool[4] { true, true, true, true };
            rule.Add(new SubRule4PointNH(relevantNeighbors, 3));
            ruleList4PNH.Add("Square fill", rule);

            rule = new List<SubRule4PointNH>();
            relevantNeighbors = new bool[4] { true, true, true, true };
            rule.Add(new SubRule4PointNH(relevantNeighbors, 1));
            relevantNeighbors = new bool[4] { true, true, true, true };
            rule.Add(new SubRule4PointNH(relevantNeighbors, 3));
            ruleList4PNH.Add("Arrows", rule);

            rule = new List<SubRule4PointNH>();
            relevantNeighbors = new bool[4] { true, false, true, true };
            rule.Add(new SubRule4PointNH(relevantNeighbors, 1));
           // rule.Add(new SubRule4PointNH(relevantNeighbors, 2));
            rule.Add(new SubRule4PointNH(relevantNeighbors, 3));
            ruleList4PNH.Add("Only three relevant", rule); 


            /*             8PNH              */

            bool[][] relevant8PNeighoburs = new bool[3][];
            for (int i = 0; i < 3; i++) { relevant8PNeighoburs[i] = new bool[3]; }
            relevant8PNeighoburs[0] = new bool[3] { false, true, false };
            relevant8PNeighoburs[1] = new bool[3] { true, false, true };
            relevant8PNeighoburs[2] = new bool[3] { false, true, false };
            List<SubRule8PointNH> rule8Test = new List<SubRule8PointNH>();
            rule8Test.Add(new SubRule8PointNH(relevant8PNeighoburs, 1));
            relevant8PNeighoburs = new bool[3][];
            relevant8PNeighoburs[0] = new bool[3] { false, true, false };
            relevant8PNeighoburs[1] = new bool[3] { true, false, true };
            relevant8PNeighoburs[2] = new bool[3] { false, true, false };
            rule8Test.Add(new SubRule8PointNH(relevant8PNeighoburs, 2));
            relevant8PNeighoburs = new bool[3][];
            relevant8PNeighoburs[0] = new bool[3] { false, true, false };
            relevant8PNeighoburs[1] = new bool[3] { true, false, true };
            relevant8PNeighoburs[2] = new bool[3] { false, true, false };
            rule8Test.Add(new SubRule8PointNH(relevant8PNeighoburs, 3));
            ruleList8PNH.Add("8PNH test rule", rule8Test);


            /*            24PNH              */
            bool[][] relevant24Neighbours = new bool[5][];
            relevant24Neighbours[0] = new bool[5] { false, true, true, true, false };
            relevant24Neighbours[1] = new bool[5] { false, true, true, true, false };
            relevant24Neighbours[2] = new bool[5] { true, true, true, true, true};
            relevant24Neighbours[3] = new bool[5] { false, true, true, true, false };
            relevant24Neighbours[4] = new bool[5] { false, true, true, true, false };
            List<SubRule24PointNH> rule24Test = new List<SubRule24PointNH>();
            rule24Test.Add(new SubRule24PointNH(relevant24Neighbours, 4));
            relevant24Neighbours = new bool[5][];
            relevant24Neighbours[0] = new bool[5] { false, true, true, true, false };
            relevant24Neighbours[1] = new bool[5] { false, true, true, true, false };
            relevant24Neighbours[2] = new bool[5] { true, true, true, true, true };
            relevant24Neighbours[3] = new bool[5] { false, true, true, true, false };
            relevant24Neighbours[4] = new bool[5] { false, true, true, true, false };
            rule24Test.Add(new SubRule24PointNH(relevant24Neighbours, 2));
            ruleList24PNH.Add("24PNH test rule", rule24Test);


        }
        public void RefreshRuleList()
        {

        }
        private void addItemsToCheckBox(int numberOfNeighbours)
        {
            ruleComboBox.Items.Clear();
            switch (numberOfNeighbours)
            {
                case 4:
                    foreach (string name in ruleList4PNH.Keys) { ruleComboBox.Items.Add(name); }
                    break;
                case 8:
                    foreach (string name in ruleList8PNH.Keys) { ruleComboBox.Items.Add(name); }
                    break;
                case 24:
                    foreach (string name in ruleList24PNH.Keys) { ruleComboBox.Items.Add(name); }
                    break;
            }
        }

        private void LifeGrid_DragOver(object sender, DragEventArgs e)
        {
            
        }

        private void LifeGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var uiElement = Mouse.DirectlyOver as Rectangle;
                Cell cell = (Cell)uiElement.DataContext;
                cell.IsAlive = true;
            }
        }

        private void EditAddRuleButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditRuleWindow addEditRuleWindow = new AddEditRuleWindow();
            addEditRuleWindow.ShowDialog();
        }

        private void addMS_Click(object sender, RoutedEventArgs e)
        {
            int currentTime = int.Parse(timeTextBox.Text);
            currentTime += 10;
            timeTextBox.Text = currentTime.ToString();
        }

        private void lowerMS_Click(object sender, RoutedEventArgs e)
        {
            int currentTime = int.Parse(timeTextBox.Text);
            currentTime -= 10;
            timeTextBox.Text = currentTime.ToString();
        }
    }
}
