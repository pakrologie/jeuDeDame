using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication2
{
    class Rule
    {
        public static Point canAtk(int y1, int x1, int y2, int x2, bool playerTop)
        {
            for (int y = 0; y < Plateau.plateauCases.Length; y++)
            {
                for (int x = 0; x < Plateau.plateauCases[y].Length; x++)
                {
                    if (Plateau.plateauCases[y][x].pawnExist)
                    {
                        int distance1 = Distance.getDistance(y, x, y1, x1);
                        int distance2 = Distance.getDistance(y, x, y2, x2);

                        if (distance1 == 1 && distance2 == 1)
                        {
                            if (playerTop != Plateau.plateauCases[y][x].pawnTop)
                            {
                                return new Point(x, y);
                            }
                        }
                    }
                }
            }
            return new Point(-1, -1);
        }

        public static void checkJumpingNotPlayed(Joueur isPlaying, int myX, int myY)
        {
            bool playerTop = isPlaying.infos.playerTop;
            for (int y1 = 0; y1 < Plateau.plateauCases.Length; y1++)
            {
                for (int x1 = 0; x1 < Plateau.plateauCases[y1].Length; x1++)
                {
                    if (Plateau.plateauCases[y1][x1].pawnTop == playerTop &&
                        Plateau.plateauCases[y1][x1].pawnExist && (Plateau.plateauCases[y1][x1].pb != Plateau.plateauCases[myY][myX].pb))
                    { // TODO : Comparaison .pb à changer
                        for (int y2 = 0; y2 < Plateau.plateauCases.Length; y2++)
                        {
                            for (int x2 = 0; x2 < Plateau.plateauCases[y2].Length; x2++)
                            {
                                if (Plateau.plateauCases[y2][x2].pawnTop != playerTop &&
                                    Plateau.plateauCases[y2][x2].pawnExist)
                                {
                                    if (!Plateau.plateauCases[y1][x1].king) // Pion normal
                                    {
                                        if (RuleAdvanced.detectCanEat(isPlaying, x1, y1))
                                        {
                                            Plateau.plateauCases[y1][x1].isnotcareful = true;
                                        }
                                    }else // Reine
                                    {
                                        if (RuleAdvanced.detectCanEatForKing(isPlaying, x1, y1))
                                        {
                                            Plateau.plateauCases[y1][x1].isnotcareful = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}