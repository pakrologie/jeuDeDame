﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class Rule
    {
        public static int x_pawn;
        public static int y_pawn;

        public static int distanceOk(int y1, int x1, int y2, int x2, bool playerTop)
        {
            int distance = getDistance(y1, x1, y2, x2);

            if (distance == 3)
            {
                if (playerTop)
                {
                    if (y1 - y2 != 2)
                        return 0;
                }
                else
                if (y2 - y1 != 2)
                    return 0;

                string[] infoPawn = canAtk(y1, x1, y2, x2, playerTop).Split(' ');
                if (infoPawn.Length != 2)
                    return 0;

                y_pawn = Convert.ToInt32(infoPawn[0]);
                x_pawn = Convert.ToInt32(infoPawn[1]);
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

        public static string canAtk(int y1, int x1, int y2, int x2, bool playerTop)
        {
            for (int y = 0; y < Plateau.plateauCases.Length; y++)
            {
                for (int x = 0; x < Plateau.plateauCases[y].Length; x++)
                {
                    if (Plateau.plateauCases[y][x].pawnExist)
                    {
                        int distance1 = getDistance(y, x, y1, x1);
                        int distance2 = getDistance(y, x, y2, x2);

                        if (distance1 == 1 && distance2 == 1)
                        {
                            if (playerTop != Plateau.plateauCases[y][x].pawnTop)
                            {
                                return String.Format("{0} {1}", y, x);
                            }
                        }
                    }
                }
            }
            return "F";
        }

        public static int getDistance(int y1, int x1, int y2, int x2)
        {
            return Convert.ToInt32(Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
        }
    }
}