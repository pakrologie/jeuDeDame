using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Jeu_De_Dame___Serveur
{
    class Maths
    {
        public static Point getCoeffDiff(int x1, int y1, int x2, int y2)
        {
            int addCoeffX = 1;
            int addCoeffY = 1;

            if (x1 < x2)
            {
                addCoeffX = -1;
            }
            if (y1 < y2)
            {
                addCoeffY = -1;
            }

            return new Point(addCoeffX, addCoeffY);
        }
    }
}
