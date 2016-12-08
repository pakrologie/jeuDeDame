using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication2
{
    class Distance
    {
        public static int xPawn;
        public static int yPawn;

        /* 
         * 0 = Distance insuffisante
         * 1 = Déplacement d'une case
         * 2 = Mange un pion
        */

        public static int distanceOk(int y1, int x1, int y2, int x2, bool playerTop)
        {
            int distance = getDistance(y1, x1, y2, x2);

            if (distance == 3)
            {
                Point OpponentCoords = Rule.canAtk(y1, x1, y2, x2, playerTop);

                if (OpponentCoords.X == -1)
                    return 0;

                yPawn = OpponentCoords.Y;
                xPawn = OpponentCoords.X;
                return 2;
            }
            else if (distance == 1)
            {
                if (playerTop)
                {
                    if (y1 - y2 != 1)
                        return 0;
                }
                else
                if (y2 - y1 != 1)
                    return 0;
                return 1;
            }

            return 0;
        }

        public static int getDistance(int y1, int x1, int y2, int x2)
        {
            return Convert.ToInt32(Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
        }
    }
}
