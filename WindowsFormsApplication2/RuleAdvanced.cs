using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class RuleAdvanced
    {
        public static void isNotCareful(int y, int x)
        {
            if (Plateau.plateauCases[y][x].isnotcareful && Plateau.plateauCases[y][x].pawnExist)
            {
                DialogResult request = MessageBox.Show("Souhaitez-vous détruire ce pion ?", "Sauté n'est pas joué", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (request == DialogResult.Yes)
                {
                    Animation.makeTransition((int)BunifuAnimatorNS.AnimationType.Particles, x, y);
                    Action.setCase(x, y);
                }
                return;
            }
            MessageBox.Show("Ce n'est pas vos pions");
        }

        public static void resetNotCarefulOpponent(Joueur Opponent)
        {
            for (int y = 0; y < Plateau.plateauCases.Length; y++)
            {
                for (int x = 0; x < Plateau.plateauCases[y].Length; x++)
                {
                    if (Opponent.infos.playerTop == Plateau.plateauCases[y][x].pawnTop)
                    {
                        Plateau.plateauCases[y][x].isnotcareful = false;
                    }
                }
            }
        }

        public static bool detectCanEatForKing(Joueur Player, int x, int y)
        {
            bool playerTop = Player.infos.playerTop;

            for (int y1 = 0; y1 < Plateau.plateauCases.Length; y1++)
            {
                for (int x1 = 0; x1 < Plateau.plateauCases[y1].Length; x1++)
                {
                    Point coeffCoords = Maths.getCoeffDiff(x1, y1, x, y);
                    int addCoeffX = coeffCoords.X;
                    int addCoeffY = coeffCoords.Y;

                    int xData = x1 + addCoeffX;
                    int yData = y1 + addCoeffY;

                    if (xData <= 9 && yData <= 9 &&
                        xData >= 0 && yData >= 0)
                    {
                        if (Distance.freeField(playerTop, xData, yData, x, y, false) == 2)
                        {
                            return true;
                        }
                    }
                }  
            }
            return false;
        }

        public static bool detectCanEat(Joueur Player, int x, int y)
        {
            bool playerTop = Player.infos.playerTop;

            for (int y1 = 0; y1 < Plateau.plateauCases.Length; y1++)
            {
                for (int x1 = 0; x1 < Plateau.plateauCases[y1].Length; x1++)
                {
                    if (Plateau.plateauCases[y1][x1].pawnTop != playerTop &&
                        Plateau.plateauCases[y1][x1].pawnExist)
                    {
                        int distance = Distance.getDistance(y, x, y1, x1);
                        if (distance == 1)
                        {
                            Point coeffCoords = Maths.getCoeffDiff(x1, y1, x, y);
                            int countDiffY = coeffCoords.Y * 2;
                            int countDiffX = coeffCoords.X * 2;

                            if ((x + countDiffX) <= 9 && (y + countDiffY) <= 9 &&
                                (x + countDiffX) >= 0 && (y + countDiffY) >= 0)
                            {
                                if (!Plateau.plateauCases[y + countDiffY][x + countDiffX].pawnExist)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static Plateau.cases getComboMain(Joueur Player)
        {
            bool playerTop = Player.infos.playerTop;
            for (int y = 0; y < Plateau.plateauCases.Length; y++)
            {
                for (int x = 0; x < Plateau.plateauCases[y].Length; x++)
                {
                    if (Plateau.plateauCases[y][x].pawnTop == Player.infos.playerTop &&
                        Plateau.plateauCases[y][x].pawnExist)
                    {
                        if (Plateau.plateauCases[y][x].mainCombo)
                        {
                            return Plateau.plateauCases[y][x];
                        }
                    }
                }
            }
            return new Plateau.cases();
        }

        public static bool canMakeAnAction(Joueur Player)
        {
            bool playerTop = Player.infos.playerTop;
            for (int y = 0; y < Plateau.plateauCases.Length; y++)
            {
                for (int x = 0; x < Plateau.plateauCases[y].Length; x++)
                {
                    if (Plateau.plateauCases[y][x].pawnTop == playerTop &&
                        Plateau.plateauCases[y][x].pawnExist)
                    {
                        int addX = 1;
                        int addY = 1;

                        if (!playerTop)
                        {
                            addX = -1;
                            addY = -1;
                        }

                        if ((x + addX * -1) <= 9 && (y + addY) <= 9 &&
                               (x + addX * -1) >= 0 && (y + addY) >= 0 ||
                               (x + addX) <= 9 && (y + addY) <= 9 &&
                               (x + addX) >= 0 && (y + addY) >= 0)
                        {
                            if (!Plateau.plateauCases[y + addY][x + (addX * -1)].pawnExist)
                            {
                                return true;
                            }

                            if (!Plateau.plateauCases[y + addY][x + addX].pawnExist)
                            {
                                return true;
                            }
                        }

                        if (detectCanEat(Player, x, y))
                        {
                            return true;
                        }
                    }   
                }
            }
            return false;
        }

        public static bool isLastLine(bool playerTop, int y)
        {
            if (playerTop)
            {
                if (y == 9)
                {
                    return true;
                }
            }else
            {
                if (y == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
