using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jeu_De_Dame___Serveur
{
    class Plateau
    {
        public struct cases
        {
            public Rectangle Rec;
            public bool isBlack;
            public PictureBox pb;
            public bool pawnExist;
            public bool pawnTop;
            public bool king;
            public bool isnotcareful;
            public bool mainCombo;
        }

        // Rempli la variable .plateau du client Player
        public static void remplirPlateau(Client Player)
        {
            int IndexClient = ClientManager.byPseudo(Player.info_main.pseudo);

            if (IndexClient == -1)
            {
                return;
            }

            bool value = false;
            int index = 0;
            int index_plateau = 0;
            int[] ligne = new int[10];

            for (int i = 0; i < 100; i++)
            {
                int loca = i + 1;

                if (loca % 10 == 0 && i != 0)
                {
                    ligne[index] = Convert.ToInt32(value);
                    cases[] nouvelleCases = new cases[10];

                    for (int e = 0; e < 10; e++)
                    {
                        nouvelleCases[e].isBlack = Convert.ToBoolean(ligne[e]);
                    }

                    ClientManager.ListClient[IndexClient].info_game.plateauCases[index_plateau] = nouvelleCases;
                    ligne = new int[10];

                    index = 0;
                    index_plateau++;
               } 
                else if (i % 2 == 0)
                {
                    ligne[index] = Convert.ToInt32(value);
                    index++;
                    value = !value;
                }
                else
                {
                    ligne[index] = Convert.ToInt32(value);
                    index++;
                    value = !value;
                }
            }
        }
    }
}
