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
    }
}
