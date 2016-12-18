using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Jeu_De_Dame___Serveur
{
    class CheckTime
    {
        static Mutex mut = new Mutex();

        public static void CheckDuration()
        {
            while (true)
            {
                mut.WaitOne();
                for (int i = 0; i < ClientManager.ListClient.Count; i++)
                {                
                    Timer(i);
                }
                mut.ReleaseMutex();
            }
            Thread.Sleep(20);
        }

        public static void Timer(int IndexClient)
        {
            if (ClientManager.ListClient[IndexClient].info_game.isplaying)
            {
                if (ClientManager.ListClient[IndexClient].info_game.tour)
                {
                    int TimeCount = (Environment.TickCount / 1000) - (ClientManager.ListClient[IndexClient].info_game.timeCount / 1000);
                    
                    if (TimeCount >= 30)
                    {
                        int IndexOpponent = ClientManager.byPseudo(ClientManager.ListClient[IndexClient].info_game.opponent);

                        if (IndexOpponent == -1)
                        {
                            return;
                        }

                        ClientManager.ListClient[IndexClient].info_game.asked = false;
                        ClientManager.ListClient[IndexOpponent].info_game.asked = false;

                        playerManager.ChangeGameTurn(ClientManager.ListClient[IndexClient]);

                        ClientManager.ListClient[IndexClient].SendMsg("Temps écoulé ! Changement de tour !");
                        ClientManager.ListClient[IndexOpponent].SendMsg("Temps écoulé ! Changement de tour !");
                    }
                }
            }
        }
    }
}
