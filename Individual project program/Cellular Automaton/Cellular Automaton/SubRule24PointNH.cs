using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellular_Automaton
{
    public class SubRule24PointNH
    {
        public bool[][] isNeighbourRelevant = new bool[3][];
        public int requiredNumberOfAliveCells;

        public SubRule24PointNH() { }
        public SubRule24PointNH(bool[][] isNeighbourRelevant, int requiredNumberOfAliveCells)
        {
            this.isNeighbourRelevant = isNeighbourRelevant;
            this.requiredNumberOfAliveCells = requiredNumberOfAliveCells;
        }
        public override string ToString()
        {
            string name = "Rel-";
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i != 2 && j != 2)
                        if (isNeighbourRelevant[i][j]) name += 1; else name += 0;
                }


            }
            name += "-Req-" + requiredNumberOfAliveCells;
            return name;
        }
    }
}
