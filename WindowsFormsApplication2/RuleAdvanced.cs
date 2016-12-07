using System;
using System.Collections.Generic;
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
                    ImagesManager.setCase(x, y);
                    MessageBox.Show("Le pion a été détruit !");
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
                        int distance = Rule.getDistance(y, x, y1, x1);
                        if (distance == 1)
                        {
                            int countDiffY = -2;
                            int countDiffX = -2;

                            if (y < y1)
                            {
                                countDiffY = 2;
                            }

                            if (x < x1)
                            {
                                countDiffX = 2;
                            }

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
    }
}
