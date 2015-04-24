using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellular_Automaton
{
    /// <summary>
    /// This class stores infromation for subrule in 4 point neighborhood. isNeighbourRelevant table, stores information about wheter given cell from neighbourhood
    /// is taken into account of number of alive cells or not.
    /// </summary>
    class SubRule4PointNH
    {
        public bool[] isNeighbourRelevant = new bool[4];
        public int requiredNumberOfAliveCells;

        public SubRule4PointNH(bool[] isNeighbourRelevant, int requiredNumberOfAliveCells)
        {
            if (isNeighbourRelevant.Count() > 4)
            {
                throw new System.ArgumentException("Table with neighbours cannot be grater than 4");
            }
            if (requiredNumberOfAliveCells > 4 || requiredNumberOfAliveCells <0)
            {
                throw new System.ArgumentException("Number of cells cannot be greater than 4 nor smaller than 0");
            }
            this.isNeighbourRelevant = isNeighbourRelevant;
            this.requiredNumberOfAliveCells = requiredNumberOfAliveCells;
            
        }
    }
}
