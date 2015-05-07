using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace Cellular_Automaton
{
    /// <summary>
    /// Interaction logic for AddEditRuleWindow.xaml
    /// </summary>
    public partial class AddEditRuleWindow : MetroWindow
    {
        public NeighborhoodType neighborhoodType;

        public List<SubRule4PointNH> FourPointNeighborhoodList;
        public List<SubRule8PointNH> EightPointNeighborhoodList;
        public List<SubRule24PointNH> TwentyFourPointNeighborhoodList;

        public SubRule4PointNH subRule4PNH = new SubRule4PointNH();
        public SubRule8PointNH subRule8PNH = new SubRule8PointNH();
        public SubRule24PointNH subRule24PNH = new SubRule24PointNH();

        public List<Label> labelList = new List<Label>();
        public AddEditRuleWindow()
        {
            InitializeComponent();
            populateRuleListCombobox();
        }
        private void InitEnviromentGrid(int neighboursCount)
        {
            int rowColNum = 0;
            if (neighboursCount == 24) { rowColNum = 5; }
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
        public void populateRuleListCombobox()
        {
            ruleListCombobox.Items.Clear();
            foreach (string name in MainWindow.ruleList4PNH.Keys) {
                String tmp = "(4PNH) ";
                ruleListCombobox.Items.Add(tmp+name);
            }
            foreach (string name in MainWindow.ruleList8PNH.Keys)
            {
                String tmp = "(8PNH) ";
                ruleListCombobox.Items.Add(tmp + name);
            }
            foreach (string name in MainWindow.ruleList24PNH.Keys)
            {
                String tmp = "(24PNH) ";
                ruleListCombobox.Items.Add(tmp + name);
            }  
        }
        private void PopulateEnviromentGrid(int neighboursCount)
        {
            labelList.Clear();
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
                        label.MouseDown += new MouseButtonEventHandler(label_MouseDown);
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

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label tmp = (Label)sender;
            int row = Grid.GetRow(tmp);
            int col = Grid.GetColumn(tmp);
            if (((SolidColorBrush) tmp.Background).Color == Colors.Yellow)
            {
                tmp.Background = new SolidColorBrush(Colors.White);
                
                if (neighborhoodType == NeighborhoodType.FourPNH)
                {
                    SetNewRelevance4PNH(row, col,false);
               //     ColorActiveCells4PNH();
                }else if(neighborhoodType == NeighborhoodType.EightPNH)
                {
                    SetNewRelevance8PNH(row, col, false);
                }
                else if (neighborhoodType == NeighborhoodType.TwentyFivePNH)
                {
                    SetNewRelevance24PNH(row, col, false);
                }
            }
            else
            {
                tmp.Background = new SolidColorBrush(Colors.Yellow);
                if (neighborhoodType == NeighborhoodType.FourPNH)
                {
                    SetNewRelevance4PNH(row, col, true);
                    //     ColorActiveCells4PNH();
                }
                else if (neighborhoodType == NeighborhoodType.EightPNH)
                {
                    SetNewRelevance8PNH(row, col, true);
                }
                else if (neighborhoodType == NeighborhoodType.TwentyFivePNH)
                {
                    SetNewRelevance24PNH(row, col, true);
                }
            }
            updateMaxNumberOfAliveCells();
            subRuleListView.Items.Refresh();
        }

        private void SetNewRelevance24PNH(int row, int col, bool relevance)
        {
            SubRule24PointNH tmpSubRule = (SubRule24PointNH)subRuleListView.SelectedItem;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (row == j && col == i)
                    {
                        tmpSubRule.isNeighbourRelevant[i][j] = relevance;
                    }
                }
            }
        }

        private void SetNewRelevance8PNH(int row, int col, bool relevance)
        {
            SubRule8PointNH tmpSubRule = (SubRule8PointNH)subRuleListView.SelectedItem;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (row == j && col == i)
                    {
                        tmpSubRule.isNeighbourRelevant[i][j] = relevance;
                    }
                }
            }
        }
        public void SetNewRelevance4PNH(int row, int col,bool relevance)
        {
            SubRule4PointNH tmpSubRule = (SubRule4PointNH)subRuleListView.SelectedItem;
            if (row == 0 && col == 1)
            {
               tmpSubRule.isNeighbourRelevant[0] = relevance;
            }
            else if (row == 1 && col == 2)
            {
                tmpSubRule.isNeighbourRelevant[1] = relevance;
            }
            else if (row == 2 && col == 1)
            {
                tmpSubRule.isNeighbourRelevant[2] = relevance;
            }
            else
                tmpSubRule.isNeighbourRelevant[3] = relevance;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitEnviromentGrid(4);
            PopulateEnviromentGrid(4);
        }

        private void ruleListCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ruleListCombobox.SelectedValue != null)
            {
                String tmp = ruleListCombobox.SelectedValue.ToString();
                String[] typeAndName = tmp.Split(new char[] { ' ' }, 2);
                String type = typeAndName[0];
                String name = typeAndName[1];
                if (type == "(4PNH)")
                {
                    FourPointNeighborhoodList = MainWindow.ruleList4PNH[name];
                    neighborhoodType = NeighborhoodType.FourPNH;
                    InitEnviromentGrid(4);
                    PopulateEnviromentGrid(4);
                }
                else if (type == "(8PNH)")
                {
                    EightPointNeighborhoodList = MainWindow.ruleList8PNH[name];
                    neighborhoodType = NeighborhoodType.EightPNH;
                    InitEnviromentGrid(8);
                    PopulateEnviromentGrid(8);
                }
                else if (type == "(24PNH)")
                {
                    TwentyFourPointNeighborhoodList = MainWindow.ruleList24PNH[name];
                    neighborhoodType = NeighborhoodType.TwentyFivePNH;
                    InitEnviromentGrid(24);
                    PopulateEnviromentGrid(24);
                }
                else
                {
                    throw new SystemException("Wrong neighborhood type i ruleCombobox_SelectionChanged");
                }
                loadSubRulesToListView();
            }
        }
        private void loadSubRulesToListView()
        {
            subRuleListView.Items.Clear();
            if (neighborhoodType == NeighborhoodType.FourPNH)
            {
                foreach (SubRule4PointNH subRule in FourPointNeighborhoodList)
                {
                    subRuleListView.Items.Add(subRule);
                }
            }
            else if (neighborhoodType == NeighborhoodType.EightPNH)
            {
                foreach (SubRule8PointNH subRule in EightPointNeighborhoodList)
                {
                    subRuleListView.Items.Add(subRule);
                }
            }
            else
            {
                foreach (SubRule24PointNH subRule in TwentyFourPointNeighborhoodList)
                {
                    subRuleListView.Items.Add(subRule);
                }
            }
        }

        public void ColorActiveCells4PNH()
        {
                Label tmp;
                tmp = (Label)enviromentGrid.Children[0];
                if (subRule4PNH.isNeighbourRelevant[0])
                {                       
                        tmp.Background = new SolidColorBrush(Colors.Yellow);
                }
                else tmp.Background = new SolidColorBrush(Colors.White);
                tmp = (Label)enviromentGrid.Children[3];
                if (subRule4PNH.isNeighbourRelevant[1])
                {
                   
                    tmp.Background = new SolidColorBrush(Colors.Yellow);
                }
                else tmp.Background = new SolidColorBrush(Colors.White);
                tmp = (Label)enviromentGrid.Children[4];
                if (subRule4PNH.isNeighbourRelevant[2])
                {
                    tmp.Background = new SolidColorBrush(Colors.Yellow);
                }
                else tmp.Background = new SolidColorBrush(Colors.White);
                tmp = (Label)enviromentGrid.Children[1];
                if (subRule4PNH.isNeighbourRelevant[3])
                {
                    tmp.Background = new SolidColorBrush(Colors.Yellow);
                }
                else tmp.Background = new SolidColorBrush(Colors.White);
            
        }
        private void ColorActiveCells8PNH()
        {
            Label tmp;
            int childCounter = 0;
            tmp = (Label)enviromentGrid.Children[childCounter];           
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!(i == 1 && j == 1))
                    {
                        tmp = (Label)enviromentGrid.Children[childCounter]; 
                        if (subRule8PNH.isNeighbourRelevant[i][j])
                        {
                            tmp.Background = new SolidColorBrush(Colors.Yellow);
                        }
                        else tmp.Background = new SolidColorBrush(Colors.White);
                    }
                    childCounter++;
                }
            }
        }
        private void ColorActiveCells24PNH()
        {
            Label tmp;
            int childCounter = 0;
            tmp = (Label)enviromentGrid.Children[childCounter];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        tmp = (Label)enviromentGrid.Children[childCounter];
                        if (subRule24PNH.isNeighbourRelevant[i][j])
                        {
                            tmp.Background = new SolidColorBrush(Colors.Yellow);
                        }
                        else tmp.Background = new SolidColorBrush(Colors.White);
                    }
                    childCounter++;
                }
            }
        }

        public enum NeighborhoodType
        {
            FourPNH,
            EightPNH,
            TwentyFivePNH
        }

        private void subRuleListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (subRuleListView.SelectedItem != null)
            {
                if (Object.ReferenceEquals(subRuleListView.SelectedItem.GetType(), subRule4PNH.GetType()))
                {
                    subRule4PNH = (SubRule4PointNH)subRuleListView.SelectedItem;
                    RequiredNumberOfAliveCellsTextBox.Text = subRule4PNH.requiredNumberOfAliveCells.ToString();
                    InitEnviromentGrid(4);
                    PopulateEnviromentGrid(4);
                    ColorActiveCells4PNH();
                }
                else if (Object.ReferenceEquals(subRuleListView.SelectedItem.GetType(), subRule8PNH.GetType()))
                {
                    subRule8PNH = (SubRule8PointNH)subRuleListView.SelectedItem;
                    RequiredNumberOfAliveCellsTextBox.Text = subRule8PNH.requiredNumberOfAliveCells.ToString();
                    InitEnviromentGrid(8);
                    PopulateEnviromentGrid(8);
                    ColorActiveCells8PNH();
                }
                else if (Object.ReferenceEquals(subRuleListView.SelectedItem.GetType(), subRule24PNH.GetType()))
                {
                    subRule24PNH = (SubRule24PointNH)subRuleListView.SelectedItem;
                    RequiredNumberOfAliveCellsTextBox.Text = subRule24PNH.requiredNumberOfAliveCells.ToString();
                    InitEnviromentGrid(24);
                    PopulateEnviromentGrid(24);
                    ColorActiveCells24PNH();
                }
            }
            updateMaxNumberOfAliveCells();
        }

        private void deselectButton_Click(object sender, RoutedEventArgs e)
        {
            if (neighborhoodType == NeighborhoodType.FourPNH)
            {
                deselect4PNH();
                updateMaxNumberOfAliveCells();
            }
            else if (neighborhoodType == NeighborhoodType.EightPNH)
            {
                deselect8PNH();
                updateMaxNumberOfAliveCells();
            }
            else if (neighborhoodType == NeighborhoodType.TwentyFivePNH)
            {
                deselect24PNH();
                updateMaxNumberOfAliveCells();
            }
        }
        private void deselect4PNH()
        {
            for (int i = 0; i < 4; i++)
            {
                subRule4PNH.isNeighbourRelevant[i] = false;
                subRule4PNH.requiredNumberOfAliveCells = 0;
                ColorActiveCells4PNH();
                subRuleListView.Items.Refresh();
            }
        }
        private void deselect8PNH()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!(i == 1 && j == 1))
                    {
                        subRule8PNH.isNeighbourRelevant[i][j] = false;
                        subRule8PNH.requiredNumberOfAliveCells = 0;
                        ColorActiveCells8PNH();
                        subRuleListView.Items.Refresh();
                    }
                }
            }
        }
        private void deselect24PNH()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        subRule8PNH.requiredNumberOfAliveCells = 0;
                        subRule24PNH.isNeighbourRelevant[i][j] = false;
                        ColorActiveCells24PNH();
                        subRuleListView.Items.Refresh();
                    }
                }
            }
        }
        private void selectAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (neighborhoodType == NeighborhoodType.FourPNH)
            {
                selectAll4PNH();
                updateMaxNumberOfAliveCells();
            }
            else if (neighborhoodType == NeighborhoodType.EightPNH)
            {
                selectAll8PNH();
                updateMaxNumberOfAliveCells();
            }
            else if (neighborhoodType == NeighborhoodType.TwentyFivePNH)
            {
                selectAll24PNH();
                updateMaxNumberOfAliveCells();
            }
        }

        private void selectAll24PNH()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        subRule24PNH.isNeighbourRelevant[i][j] = true;
                        ColorActiveCells24PNH();
                        subRuleListView.Items.Refresh();
                    }
                }
            }
        }

        private void selectAll8PNH()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!(i == 1 && j == 1))
                    {
                        subRule8PNH.isNeighbourRelevant[i][j] = true;
                        ColorActiveCells8PNH();
                        subRuleListView.Items.Refresh();
                    }
                }
            }
        }

        private void selectAll4PNH()
        {
            for (int i = 0; i < 4; i++)
            {
                subRule4PNH.isNeighbourRelevant[i] = true;
                ColorActiveCells4PNH();
                subRuleListView.Items.Refresh();
            }
        }

        private void updateMaxNumberOfAliveCells()
        {
            int numOfCellsAlive = 0;
            if (subRuleListView.SelectedItem != null)
            {               
                if (neighborhoodType == NeighborhoodType.FourPNH)
                {
                    for (int i = 0; i < 4; i++) { if (subRule4PNH.isNeighbourRelevant[i] == true)numOfCellsAlive++; }
                }
                else if (neighborhoodType == NeighborhoodType.EightPNH)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (!(i == 1 && j == 1))
                            {
                                if (subRule8PNH.isNeighbourRelevant[i][j] == true) { numOfCellsAlive++; }
                            }
                        }
                    }
                }
                else if (neighborhoodType == NeighborhoodType.TwentyFivePNH)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if (!(i==2 && j==2))
                            {
                                if (subRule24PNH.isNeighbourRelevant[i][j] == true) { numOfCellsAlive++; }
                            }
                        }
                    }
                }
            }
            maxNumberOfCellsAliveTextBox.Text = numOfCellsAlive.ToString();
        }

        private void RequiredNumberOfAliveCellsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RequiredNumberOfAliveCellsTextBox.Text != "" && RequiredNumberOfAliveCellsTextBox.Text != null && maxNumberOfCellsAliveTextBox.Text != "")
            {
                int requiredNumberOfCellsAlive;
                int maxNumOfCellsAlive;

                requiredNumberOfCellsAlive = int.Parse(RequiredNumberOfAliveCellsTextBox.Text);
                maxNumOfCellsAlive = int.Parse(maxNumberOfCellsAliveTextBox.Text);
                if (requiredNumberOfCellsAlive > maxNumOfCellsAlive)
                {
                    MessageBox.Show("Required number of cells alive cannot be grater than " + maxNumberOfCellsAliveTextBox + ".");
                   // RequiredNumberOfAliveCellsTextBox.Text = "0";
                 }
                else
                {
                    if (neighborhoodType == NeighborhoodType.FourPNH)
                    {
                        subRule4PNH.requiredNumberOfAliveCells = requiredNumberOfCellsAlive;
                        subRuleListView.Items.Refresh();
                    }
                    else if (neighborhoodType == NeighborhoodType.EightPNH)
                    {
                        subRule8PNH.requiredNumberOfAliveCells = requiredNumberOfCellsAlive;
                        subRuleListView.Items.Refresh();
                    }
                    else
                    {
                        subRule24PNH.requiredNumberOfAliveCells = requiredNumberOfCellsAlive;
                        subRuleListView.Items.Refresh();
                    }
                }
            }
               
        }

        private void addNewRuleButton_Click(object sender, RoutedEventArgs e)
        {
            AddRule addRule = new AddRule(this);
            addRule.ShowDialog();
        }
        public void  UpdateWindowAfterRuleAdding(){

        }

        private void addSubRule_Click(object sender, RoutedEventArgs e)
        {
            if (neighborhoodType == NeighborhoodType.FourPNH)
            {
                if (FourPointNeighborhoodList != null)
                {
                    bool[] relevantNeighbors = new bool[4] { true, true, true, true };
                    FourPointNeighborhoodList.Add(new SubRule4PointNH(relevantNeighbors, 4));
                    loadSubRulesToListView();
                }
            }
            else if (neighborhoodType == NeighborhoodType.EightPNH)
            {
                bool[][] relevant8PNeighoburs = new bool[3][];
                for (int i = 0; i < 3; i++) { relevant8PNeighoburs[i] = new bool[3]; }
                relevant8PNeighoburs[0] = new bool[3] { false, true, false };
                relevant8PNeighoburs[1] = new bool[3] { true, false, true };
                relevant8PNeighoburs[2] = new bool[3] { false, true, false };
                EightPointNeighborhoodList.Add(new SubRule8PointNH(relevant8PNeighoburs, 3));
                loadSubRulesToListView();
            }
            else if (neighborhoodType == NeighborhoodType.TwentyFivePNH)
            {
                bool[][] relevant24Neighbours = new bool[5][];
                relevant24Neighbours[0] = new bool[5] { false, true, true, true, false };
                relevant24Neighbours[1] = new bool[5] { false, true, true, true, false };
                relevant24Neighbours[2] = new bool[5] { true, true, true, true, true };
                relevant24Neighbours[3] = new bool[5] { false, true, true, true, false };
                relevant24Neighbours[4] = new bool[5] { false, true, true, true, false };
                TwentyFourPointNeighborhoodList.Add(new SubRule24PointNH(relevant24Neighbours, 4));
                loadSubRulesToListView();
            }
        }

        private void ereaseSubRule_Click(object sender, RoutedEventArgs e)
        {
            if (neighborhoodType == NeighborhoodType.FourPNH)
            {
                if (FourPointNeighborhoodList != null)
                {
                    FourPointNeighborhoodList.Remove(subRule4PNH);
                    loadSubRulesToListView();
                }
            }
            else if (neighborhoodType == NeighborhoodType.EightPNH)
            {
                EightPointNeighborhoodList.Remove(subRule8PNH);
                loadSubRulesToListView();
            }
            else if (neighborhoodType == NeighborhoodType.TwentyFivePNH)
            {
                TwentyFourPointNeighborhoodList.Remove(subRule24PNH);
                loadSubRulesToListView();
            }
        }

        private void deleteRule_Click(object sender, RoutedEventArgs e)
        {
            if (neighborhoodType == NeighborhoodType.FourPNH)
            {
                if (FourPointNeighborhoodList != null)
                {
                    string selectedName = ruleListCombobox.SelectedValue.ToString();
                    string[] tmp = selectedName.Split(new char[] { ' ' }, 2);
                    MainWindow.ruleList4PNH.Remove(tmp[1]);
                }
            }
            else if (neighborhoodType == NeighborhoodType.EightPNH)
            {
                EightPointNeighborhoodList.Remove(subRule8PNH);
                loadSubRulesToListView();
            }
            else if (neighborhoodType == NeighborhoodType.TwentyFivePNH)
            {
                TwentyFourPointNeighborhoodList.Remove(subRule24PNH);
                loadSubRulesToListView();
            }
        }
    }
}
