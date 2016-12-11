using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class Careful
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
                                        if (Attack.detectCanAtk(isPlaying, x1, y1))
                                        {
                                            Plateau.plateauCases[y1][x1].isnotcareful = true;
                                        }
                                    }
                                    else // Reine
                                    {
                                        if (Attack.detectCanAtkForKing(isPlaying, x1, y1))
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
