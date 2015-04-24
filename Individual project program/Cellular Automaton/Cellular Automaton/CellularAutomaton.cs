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
                                    if (orginal[i][orginal[0].Count-1].IsAlive)
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
                                tmpCellList.Add(new Cell(){IsAlive=true});
                                break;
                            }
                        }
                        if(k==rules.Count){
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

        public async Task CalualtePartOfAMatrix_4PNH(List<List<Cell>> orginal, List<SubRule4PointNH> rules,int startX,int endX,int startY, int endY)
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
                        orginal[i][j].IsAlive = nextGeneration[i][j].IsAlive;
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
