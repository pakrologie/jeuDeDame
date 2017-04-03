using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jeu_De_Dame___Serveur
{
    class playerManager
    {
        public static void ChangeGameTurn(Client IsPlaying)
        {
            int IndexOpponent = ClientManager.byPseudo(IsPlaying.info_game.opponent);
            int IndexClient = ClientManager.byPseudo(IsPlaying.info_main.pseudo);

            if (IndexClient == -1 || IndexOpponent == -1)
            {
                return;
            }

            ClientManager.ListClient[IndexClient].info_game.timeCount = Environment.TickCount;
            ClientManager.ListClient[IndexOpponent].info_game.timeCount = Environment.TickCount;

            if (IsPlaying.info_game.tour)
            {
                ClientManager.ListClient[IndexClient].info_game.tour = !ClientManager.ListClient[IndexClient].info_game.tour;
                ClientManager.ListClient[IndexOpponent].info_game.tour = !ClientManager.ListClient[IndexOpponent].info_game.tour;
            }
            
            if (!EndGame.canMakeAnAction(WhosNext(IsPlaying)))
            {
                ClientManager.ListClient[IndexClient].SendMsg("Partie terminée");
                ClientManager.ListClient[IndexOpponent].SendMsg("Partie terminée");
                ClientManager.RedirectEnding(IsPlaying, false);
            }
        }

        public static Client WhosNext(Client IsPlaying)
        {
            int IndexOpponent = ClientManager.byPseudo(IsPlaying.info_game.opponent);
            int IndexClient = ClientManager.byPseudo(IsPlaying.info_main.pseudo);

            if (IndexClient == -1 || IndexOpponent == -1)
            {
                return null;
            }

            if (ClientManager.ListClient[IndexClient].info_game.tour)
            {
                return ClientManager.ListClient[IndexClient];
            }
            return ClientManager.ListClient[IndexOpponent];
        }

        public static Client GetOpponent(Client Player)
        {
            int IndexOpponent = ClientManager.byPseudo(Player.info_game.opponent);
            if (IndexOpponent == -1)
            {
                return null;
            }

            return ClientManager.ListClient[IndexOpponent];
        }
    }
}
