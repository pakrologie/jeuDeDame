using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jeu_De_Dame___Serveur
{
    class imageManager
    {
        public static void initializePawn(Client isPlaying)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);

            if (IndexClient == -1)
            {
                return;
            }

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].isBlack)
                    {
                        PictureBox pb = new PictureBox();
                        ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].Rec.X = x;
                        ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].Rec.Y = y;
                        
                        if (y < 3) // Pion du haut
                        {
                            ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnExist = true;
                            ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnTop = true;

                        }
                        if (y > 6) // Pion du bas
                        {
                            ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnExist = true;
                            ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnTop = false;
                        }
                    }
                }
            }
        }

        public static void pawnToKing(Client isPlaying, bool playerTop, int x, int y)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);

            if (IndexClient == -1)
            {
                return;
            }

            if (Distance.isLastLine(playerTop, y))
            {
                if (!ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].king)
                {
                    Action.setCase(isPlaying, x, y, getPawnImgByPlayer(playerTop, true), true, playerTop);
                    ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].king = true;
                }
            }
            Match.SynchroWithOpponents(isPlaying);
        }

        public static string getPawnImgByPlayer(bool playerTop, bool isking = false)
        {
            if (playerTop)
            {
                if (isking)
                {
                    return ("pion_1_queen.png");
                }
                return ("pion_1.png");
            }
            if (isking)
            {
                return ("pion_2_queen.png");
            }
            return ("pion_2.png");
        }
    }
}
