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
    /// Interaction logic for AddRule.xaml
    /// </summary>
    public partial class AddRule : Window
    {
        private AddEditRuleWindow addEditRuleWindow;
        public AddRule(AddEditRuleWindow addEditRuleWindow)
        {
            this.addEditRuleWindow = addEditRuleWindow;
            InitializeComponent();
        }

        private void FourPNHCHeckBox_Click(object sender, RoutedEventArgs e)
        {
            FourPNHCHeckBox.IsChecked = true;
            EightPNHCHeckBox.IsChecked = false;
            TwentyFivePNHCHeckBox.IsChecked = false;
            InitEnviromentGrid(4);
            PopulateEnviromentGrid(4);


        }

        private void EightPNHCHeckBox_Click(object sender, RoutedEventArgs e)
        {
            FourPNHCHeckBox.IsChecked = false;
            EightPNHCHeckBox.IsChecked = true;
            TwentyFivePNHCHeckBox.IsChecked = false;
            InitEnviromentGrid(8);
            PopulateEnviromentGrid(8);

        }

        private void TwentyFivePNHCHeckBox_Click(object sender, RoutedEventArgs e)
        {
            FourPNHCHeckBox.IsChecked = false;
            EightPNHCHeckBox.IsChecked = false;
            TwentyFivePNHCHeckBox.IsChecked = true;
            InitEnviromentGrid(24);
            PopulateEnviromentGrid(24);


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
        private void PopulateEnviromentGrid(int neighboursCount)
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

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)FourPNHCHeckBox.IsChecked)
            {
                add4PNH();
            }
            else if ((bool)EightPNHCHeckBox.IsChecked)
            {
                add8PNH();
            }else if((bool)TwentyFivePNHCHeckBox.IsChecked){
                add24PNH();
            }
        }

        private void add4PNH()
        {
            List<SubRule4PointNH> rule = new List<SubRule4PointNH>();
            bool[] relevantNeighbors = new bool[4] { true, true, true, true };
            rule.Add(new SubRule4PointNH(relevantNeighbors, 4));
            if (nameTextBox.Text == null || nameTextBox.Text == "")
            {
                MessageBox.Show("Name of rule is obligatory");
                return;
            }
            else
            {
                MainWindow.ruleList4PNH.Add(nameTextBox.Text, rule);
                addEditRuleWindow.populateRuleListCombobox();
                this.Close();
            }
        }
        private void add8PNH()
        {
            bool[][] relevant8PNeighoburs = new bool[3][];
            for (int i = 0; i < 3; i++) { relevant8PNeighoburs[i] = new bool[3]; }
            relevant8PNeighoburs[0] = new bool[3] { true, true, true };
            relevant8PNeighoburs[1] = new bool[3] { true, true, true };
            relevant8PNeighoburs[2] = new bool[3] { true, true, true };
            List<SubRule8PointNH> rule = new List<SubRule8PointNH>();
            rule.Add(new SubRule8PointNH(relevant8PNeighoburs, 8));
            if (nameTextBox.Text == null || nameTextBox.Text == "")
            {
                MessageBox.Show("Name of rule is obligatory");
                return;
            }
            else
            {
                MainWindow.ruleList8PNH.Add(nameTextBox.Text, rule);
                addEditRuleWindow.populateRuleListCombobox();
                this.Close();
            }
        }

        private void add24PNH()
        {
            bool[][] relevant24Neighbours = new bool[5][];
            relevant24Neighbours[0] = new bool[5] { true, true, true, true, true };
            relevant24Neighbours[1] = new bool[5] { true, true, true, true, true };
            relevant24Neighbours[2] = new bool[5] { true, true, true, true, true };
            relevant24Neighbours[3] = new bool[5] { true, true, true, true, true };
            relevant24Neighbours[4] = new bool[5] { true, true, true, true, true };
            List<SubRule24PointNH> rule = new List<SubRule24PointNH>();
            rule.Add(new SubRule24PointNH(relevant24Neighbours, 24));
            if (nameTextBox.Text == null || nameTextBox.Text == "")
            {
                MessageBox.Show("Name of rule is obligatory");
                return;
            }
            else
            {
                MainWindow.ruleList24PNH.Add(nameTextBox.Text, rule);
                addEditRuleWindow.populateRuleListCombobox();
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FourPNHCHeckBox.IsChecked = true; 
            InitEnviromentGrid(4);
            PopulateEnviromentGrid(4);
        }
    }
}
