using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeu_De_Dame___Serveur
{
    class Match
    {
        public static bool startGame(int IndexJoueur1, int IndexJoueur2)
        {
            string pseudoTop;
            string pseudoBot;

            if (ClientManager.ListClient[IndexJoueur1].info_main.playerTop ==
                ClientManager.ListClient[IndexJoueur2].info_main.playerTop)
            {
                return false;
            }

			if (ClientManager.ListClient[IndexJoueur1].info_game.opponent !=
                ClientManager.ListClient[IndexJoueur2].info_main.pseudo ||
                ClientManager.ListClient[IndexJoueur2].info_game.opponent !=
                ClientManager.ListClient[IndexJoueur1].info_main.pseudo)
            {
                return false;
            }

			/*if (!ClientManager.ListClient[IndexJoueur1].info_main.iswait ||
               !ClientManager.ListClient[IndexJoueur2].info_main.iswait)
            {
                return false;
            }*/

			if (ClientManager.ListClient[IndexJoueur1].info_game.isplaying ||
                ClientManager.ListClient[IndexJoueur2].info_game.isplaying)
            {
                return false;
            }

			if (ClientManager.ListClient[IndexJoueur1].info_main.playerTop)
            {
                pseudoTop = ClientManager.ListClient[IndexJoueur1].info_main.pseudo;
                pseudoBot = ClientManager.ListClient[IndexJoueur2].info_main.pseudo;
            }else
            {
                pseudoTop = ClientManager.ListClient[IndexJoueur2].info_main.pseudo;
                pseudoBot = ClientManager.ListClient[IndexJoueur1].info_main.pseudo;
            }

			ClientManager.ListClient[IndexJoueur1].info_game.timeCount = Environment.TickCount;
            ClientManager.ListClient[IndexJoueur2].info_game.timeCount = Environment.TickCount;

            ClientManager.ListClient[IndexJoueur1].info_game.isplaying = true;
            ClientManager.ListClient[IndexJoueur2].info_game.isplaying = true;

            ClientManager.ListClient[IndexJoueur1].info_game.pawnAlive = 15;
            ClientManager.ListClient[IndexJoueur2].info_game.pawnAlive = 15;

            ClientManager.ListClient[IndexJoueur1].info_game.plateauCases = new Plateau.cases[10][];
            ClientManager.ListClient[IndexJoueur2].info_game.plateauCases = new Plateau.cases[10][];

            Plateau.remplirPlateau(ClientManager.ListClient[IndexJoueur1]);
            imageManager.initializePawn(ClientManager.ListClient[IndexJoueur1]);
 
            SynchroWithOpponents(ClientManager.ListClient[IndexJoueur1]);
   
            ClientManager.ListClient[IndexJoueur1].Send("match " + ClientManager.ListClient[IndexJoueur2].info_main.pseudo);
            ClientManager.ListClient[IndexJoueur2].Send("match " + ClientManager.ListClient[IndexJoueur1].info_main.pseudo);

            return true;
        }

        public static void SynchroWithOpponents(Client isPlaying)
        {
            int IndexOpponent = ClientManager.byPseudo(isPlaying.info_game.opponent);

			if (IndexOpponent == -1)
			{
				return;
			}

            ClientManager.ListClient[IndexOpponent].info_game.plateauCases = isPlaying.info_game.plateauCases;
        }
    }
}