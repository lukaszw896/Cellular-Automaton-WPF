using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cellular_Automaton
{
    class CellularAutomaton
    {
        public static System.Object lockThis = new System.Object();

        private List<List<Cell>> _cellGrid = null;
        public List<List<Cell>> CellGrid
        {
            get
            {
                if (_cellGrid != null)
                    return _cellGrid;
                else
                    throw (new System.InvalidOperationException("LifeCell_get"));
            }
        }

        public CellularAutomaton(int rowNum, int colNum)
        {
            _cellGrid = new List<List<Cell>>();
            for (int i = 0; i < rowNum; i++)
            {
                List<Cell> tmpCellList = new List<Cell>();
                for (int j = 0; j < colNum; j++)
                {
                    tmpCellList.Add(new Cell());
                }
                _cellGrid.Add(tmpCellList);
            }
        }
        
        public static void ClearGrid(List<List<Cell>> grid){
            for(int i=0; i<grid.Count;i++){
                for(int j=0; j<grid[0].Count;j++){
                    grid[i][j].IsAlive=false;
                }
            }
        }

        public static async Task CalculateNewGeneration_4PNH(List<List<Cell>> orginal, List<SubRule4PointNH> rules, int timeMS)
        {
            await Task.Run(()=>
                {
                    lock (CellularAutomaton.lockThis)
                    {

                        List<List<Cell>> nextGeneration = new List<List<Cell>>();
                        for (int i = 0; i < orginal.Count; i++)
                        {
                            List<Cell> tmpCellList = new List<Cell>();
                            for (int j = 0; j < orginal[0].Count; j++)
                            {
                                int k;
                                for (k = 0; k < rules.Count; k++)
                                {
                                    int numberOfCellsAlive = 0;
                                    //North
                                    if (rules[k].isNeighbourRelevant[0])
                                    {
                                        if (j == 0)
                                        {
                                            if (orginal[i][orginal[0].Count - 1].IsAlive)
                                                numberOfCellsAlive += 1;
                                        }
                                        else
                                        {
                                            if (orginal[i][j - 1].IsAlive)
                                                numberOfCellsAlive += 1;
                                        }
                                    }
                                    //East
                                    if (rules[k].isNeighbourRelevant[1])
                                    {
                                        if (i == orginal.Count - 1)
                                        {
                                            if (orginal[0][j].IsAlive)
                                                numberOfCellsAlive += 1;
                                        }
                                        else
                                        {
                                            if (orginal[i + 1][j].IsAlive)
                                                numberOfCellsAlive += 1;
                                        }
                                    }
                                    //South
                                    if (rules[k].isNeighbourRelevant[2])
                                    {
                                        if (j == orginal[0].Count - 1)
                                        {
                                            if (orginal[i][0].IsAlive)
                                                numberOfCellsAlive += 1;
                                        }
                                        else
                                        {
                                            if (orginal[i][j + 1].IsAlive)
                                                numberOfCellsAlive += 1;
                                        }
                                    }
                                    //West
                                    if (rules[k].isNeighbourRelevant[3])
                                    {
                                        if (i == 0)
                                        {
                                            if (orginal[orginal.Count - 1][j].IsAlive)
                                                numberOfCellsAlive += 1;
                                        }
                                        else
                                        {
                                            if (orginal[i - 1][j].IsAlive)
                                                numberOfCellsAlive += 1;
                                        }
                                    }
                                    if (numberOfCellsAlive == rules[k].requiredNumberOfAliveCells)
                                    {
                                        tmpCellList.Add(new Cell() { IsAlive = true });
                                        break;
                                    }
                                }
                                if (k == rules.Count)
                                {
                                    tmpCellList.Add(new Cell());
                                }
                            }
                            nextGeneration.Add(tmpCellList);
                        }
                        //saving new generation to the matrix
                        for (int i = 0; i < orginal.Count; i++)
                        {
                            for (int j = 0; j < orginal[0].Count; j++)
                            {
                                orginal[i][j].IsAlive = nextGeneration[i][j].IsAlive;
                            }
                        }
                        Console.WriteLine("Going to sleep");
                        System.Threading.Thread.Sleep(timeMS);
                        Console.WriteLine("end of sleep");
                    }
            });
        }

        public static async Task CalculateNewGeneration_8PNH(List<List<Cell>> orginal, List<SubRule8PointNH> rules, int timeMS)
        {
            await Task.Run(() =>
            {
                lock (CellularAutomaton.lockThis)
                {

                    List<List<Cell>> nextGeneration = new List<List<Cell>>();
                    for (int i = 0; i < orginal.Count; i++)
                    {
                        List<Cell> tmpCellList = new List<Cell>();
                        for (int j = 0; j < orginal[0].Count; j++)
                        {
                            int k;
                            for (k = 0; k < rules.Count; k++)
                            {
                                int numberOfCellsAlive = 0;
                                for (int l = -1; l < 2; l++)
                                {
                                    for (int m = -1; m < 2; m++)
                                    {
                                        if (rules[k].isNeighbourRelevant[l + 1][m + 1])
                                        {
                                            if (!(m == 0 && l == 0))
                                            {
                                                int tmpX = i + l;
                                                int tmpY = j + m;
                                                if (tmpX < 0) { tmpX = orginal.Count - 1; }
                                                if (tmpX > orginal.Count - 1) { tmpX = 0; }
                                                if (tmpY < 0) { tmpY = orginal[0].Count - 1; }
                                                if (tmpY > orginal[0].Count - 1) { tmpY = 0; }

                                                if (orginal[tmpX][tmpY].IsAlive)
                                                {
                                                    numberOfCellsAlive++;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (numberOfCellsAlive == rules[k].requiredNumberOfAliveCells)
                                {
                                    tmpCellList.Add(new Cell() { IsAlive = true });
                                    break;
                                }
                            }
                            if (k == rules.Count)
                            {
                                tmpCellList.Add(new Cell());
                            }
                        }
                        nextGeneration.Add(tmpCellList);
                    }
                    //saving new generation to the matrix
                    for (int i = 0; i < orginal.Count; i++)
                    {
                        for (int j = 0; j < orginal[0].Count; j++)
                        {
                            orginal[i][j].IsAlive = nextGeneration[i][j].IsAlive;
                        }
                    }
                    Console.WriteLine("Going to sleep");
                    System.Threading.Thread.Sleep(timeMS);
                    Console.WriteLine("end of sleep");
                }
            });
        }
        internal static async Task CalculateNewGeneration_24PNH(List<List<Cell>> orginal, List<SubRule24PointNH> rules, int timeMS)
        {
            await Task.Run(() =>
            {


                List<List<Cell>> nextGeneration = new List<List<Cell>>();
                for (int i = 0; i < orginal.Count; i++)
                {
                    List<Cell> tmpCellList = new List<Cell>();
                    for (int j = 0; j < orginal[0].Count; j++)
                    {
                        int k;
                        for (k = 0; k < rules.Count; k++)
                        {
                            int numberOfCellsAlive = 0;
                            for (int l = -2; l < 3; l++)
                            {
                                for (int m = -2; m < 3; m++)
                                {
                                    if (rules[k].isNeighbourRelevant[l + 2][m + 2])
                                    {
                                        if (!(m == 0 && l == 0))
                                        {
                                            int tmpX = i + l;
                                            int tmpY = j + m;
                                            if (tmpX < 0) { tmpX = orginal.Count - 1; }
                                            if (tmpX > orginal.Count - 1) { tmpX = 0; }
                                            if (tmpY < 0) { tmpY = orginal[0].Count - 1; }
                                            if (tmpY > orginal[0].Count - 1) { tmpY = 0; }

                                            if (orginal[tmpX][tmpY].IsAlive)
                                            {
                                                numberOfCellsAlive++;
                                            }
                                        }
                                    }
                                }
                            }

                            if (numberOfCellsAlive == rules[k].requiredNumberOfAliveCells)
                            {
                                tmpCellList.Add(new Cell() { IsAlive = true });
                                break;
                            }
                        }
                        if (k == rules.Count)
                        {
                            tmpCellList.Add(new Cell());
                        }
                    }
                    nextGeneration.Add(tmpCellList);
                }
                //saving new generation to the matrix
                for (int i = 0; i < orginal.Count; i++)
                {
                    for (int j = 0; j < orginal[0].Count; j++)
                    {
                        orginal[i][j].IsAlive = nextGeneration[i][j].IsAlive;
                    }
                }
                System.Threading.Thread.Sleep(timeMS);
            });
        }


        /// <summary>
        /// multi-threading first try
        /// </summary>
        /// <param name="orginal"></param>
        /// <param name="rules"></param>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        /// <param name="startY"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        public static async Task CalualtePartOfAMatrix_4PNH(List<List<Cell>> orginal, List<SubRule4PointNH> rules, int startX, int endX, int startY, int endY)
        {
            await Task.Run(() =>
            {


                List<List<Cell>> nextGeneration = new List<List<Cell>>();
                for (int i = startX; i < endX; i++)
                {
                    List<Cell> tmpCellList = new List<Cell>();
                    for (int j = startY; j < endY; j++)
                    {
                        int k;
                        for (k = 0; k < rules.Count; k++)
                        {
                            int numberOfCellsAlive = 0;
                            //North
                            if (rules[k].isNeighbourRelevant[0])
                            {
                                if (j == 0)
                                {
                                    if (orginal[i][orginal[0].Count - 1].IsAlive)
                                        numberOfCellsAlive += 1;
                                }
                                else
                                {
                                    if (orginal[i][j - 1].IsAlive)
                                        numberOfCellsAlive += 1;
                                }
                            }
                            //East
                            if (rules[k].isNeighbourRelevant[1])
                            {
                                if (i == orginal.Count - 1)
                                {
                                    if (orginal[0][j].IsAlive)
                                        numberOfCellsAlive += 1;
                                }
                                else
                                {
                                    if (orginal[i + 1][j].IsAlive)
                                        numberOfCellsAlive += 1;
                                }
                            }
                            //South
                            if (rules[k].isNeighbourRelevant[2])
                            {
                                if (j == orginal[0].Count - 1)
                                {
                                    if (orginal[i][0].IsAlive)
                                        numberOfCellsAlive += 1;
                                }
                                else
                                {
                                    if (orginal[i][j + 1].IsAlive)
                                        numberOfCellsAlive += 1;
                                }
                            }
                            //West
                            if (rules[k].isNeighbourRelevant[3])
                            {
                                if (i == 0)
                                {
                                    if (orginal[orginal.Count - 1][j].IsAlive)
                                        numberOfCellsAlive += 1;
                                }
                                else
                                {
                                    if (orginal[i - 1][j].IsAlive)
                                        numberOfCellsAlive += 1;
                                }
                            }
                            if (numberOfCellsAlive == rules[k].requiredNumberOfAliveCells)
                            {
                                tmpCellList.Add(new Cell() { IsAlive = true });
                                break;
                            }
                        }
                        if (k == rules.Count)
                        {
                            tmpCellList.Add(new Cell());
                        }
                    }
                    nextGeneration.Add(tmpCellList);
                }
                //saving new generation to the matrix
                for (int i = startX; i < endX; i++)
                {
                    for (int j = startY; j < endY; j++)
                    {
                        orginal[i][j].IsAlive = nextGeneration[i - startX][j - startY].IsAlive;
                    }
                }
                //  System.Threading.Thread.Sleep(timeMS);
            });
        }

    }
    class Cell : INotifyPropertyChanged
    {
        private bool _isAlive = false;
        private GridLength _height = new GridLength(1.0, GridUnitType.Star);

        public Cell() { }
        public Cell(Cell cell)
        {
            this._height = cell._height;
            this._isAlive = cell._isAlive;
        }
        public bool IsAlive
        {
            get
            {
                return _isAlive;
            }
            set
            {
                _isAlive = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsAlive"));
            }
        }
        public GridLength Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Height"));
            }
        }
       
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
