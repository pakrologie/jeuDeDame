using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jeu_De_Dame___Serveur
{
    class Careful
    {
        public static void isNotCareful(Client isPlaying, int y, int x)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);

            if (IndexClient == -1)
            {
                return;
            }

            if (ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].isnotcareful && ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnExist)
            {
                /*DialogResult request = MessageBox.Show("Souhaitez-vous détruire ce pion ?", "Sauté n'est pas joué", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (request == DialogResult.Yes)
                {
                    //Animation.makeTransition((int)BunifuAnimatorNS.AnimationType.Particles, x, y);
                    Action.setCase(isPlaying, x, y);
                }*/
                isPlaying.SendRequest("Souhaitez-vous détruire ce pion ?", MessageBoxButtons.YesNo, MessageBoxIcon.Information, x, y);
                isPlaying.info_game.asked = true;
                return;
            }
            isPlaying.SendMsg("Ce n'est pas vos pions");
        }

        public static void resetNotCarefulOpponent(Client Opponent)
        {
            int IndexOpponent = ClientManager.byPseudo(Opponent.info_main.pseudo);

            if (IndexOpponent == -1)
            {
                return;
            }

            for (int y = 0; y < ClientManager.ListClient[IndexOpponent].info_game.plateauCases.Length; y++)
            {
                for (int x = 0; x < ClientManager.ListClient[IndexOpponent].info_game.plateauCases[y].Length; x++)
                {
                    if (Opponent.info_main.playerTop == ClientManager.ListClient[IndexOpponent].info_game.plateauCases[y][x].pawnTop)
                    {
                        ClientManager.ListClient[IndexOpponent].info_game.plateauCases[y][x].isnotcareful = false;
                    }
                }
            }
            Match.SynchroWithOpponents(Opponent);
        }

        public static void checkJumpingNotPlayed(Client isPlaying, int myX, int myY)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);

            if (IndexClient == -1)
            {
                return;
            }

            bool playerTop = isPlaying.info_main.playerTop;
            for (int y1 = 0; y1 < ClientManager.ListClient[IndexClient].info_game.plateauCases.Length; y1++)
            {
                for (int x1 = 0; x1 < ClientManager.ListClient[IndexClient].info_game.plateauCases[y1].Length; x1++)
                {
                    if (ClientManager.ListClient[IndexClient].info_game.plateauCases[y1][x1].pawnTop == playerTop &&
                        ClientManager.ListClient[IndexClient].info_game.plateauCases[y1][x1].pawnExist && (ClientManager.ListClient[IndexClient].info_game.plateauCases[y1][x1].Rec != ClientManager.ListClient[IndexClient].info_game.plateauCases[myY][myX].Rec))
                    {
                        for (int y2 = 0; y2 < ClientManager.ListClient[IndexClient].info_game.plateauCases.Length; y2++)
                        {
                            for (int x2 = 0; x2 < ClientManager.ListClient[IndexClient].info_game.plateauCases[y2].Length; x2++)
                            {
                                if (ClientManager.ListClient[IndexClient].info_game.plateauCases[y2][x2].pawnTop != playerTop &&
                                    ClientManager.ListClient[IndexClient].info_game.plateauCases[y2][x2].pawnExist)
                                {
                                    if (!ClientManager.ListClient[IndexClient].info_game.plateauCases[y1][x1].king) // Pion normal
                                    {
                                        if (Attack.detectCanAtk(isPlaying, x1, y1))
                                        {
                                            ClientManager.ListClient[IndexClient].info_game.plateauCases[y1][x1].isnotcareful = true;
                                            
                                        }
                                    }
                                    else // Reine
                                    {
                                        if (Attack.detectCanAtkForKing(isPlaying, x1, y1))
                                        {
                                            ClientManager.ListClient[IndexClient].info_game.plateauCases[y1][x1].isnotcareful = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Match.SynchroWithOpponents(isPlaying);
        }
    }
}
