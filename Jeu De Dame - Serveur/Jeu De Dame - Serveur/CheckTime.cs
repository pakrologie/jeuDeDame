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
					TimerTour(i);
					TimerChat(i);
                }

                mut.ReleaseMutex();

                Thread.Sleep(100);
            }
        }

		public static void TimerChat(int IndexClient)
		{
			if (ClientManager.ListClient[IndexClient].info_game.isplaying)
			{
				if (!ClientManager.ListClient[IndexClient].info_game.chatOn)
				{
					int TimeCount = (Environment.TickCount / 1000) - (ClientManager.ListClient[IndexClient].info_game.timeChatCount / 1000);
					if (TimeCount >= 3)
					{
						ClientManager.ListClient[IndexClient].info_game.chatOn = true;
					}
				}
			}
		}
        public static void TimerTour(int IndexClient)
        {
            if (ClientManager.ListClient[IndexClient].info_game.isplaying)
            {
                if (ClientManager.ListClient[IndexClient].info_game.tour)
                {
                    int TimeCount = (Environment.TickCount / 1000) - (ClientManager.ListClient[IndexClient].info_game.timeTourCount / 1000);
					int TimeExist = ClientManager.ListClient[IndexClient].info_game.timeExist;

                    if (TimeCount >= 30)
                    {
                        int IndexOpponent = ClientManager.byPseudo(ClientManager.ListClient[IndexClient].info_game.opponent);

                        if (IndexOpponent == -1)
                        {
                            return;
                        }

                        ClientManager.ListClient[IndexClient].info_game.asked = false;
                        ClientManager.ListClient[IndexOpponent].info_game.asked = false;

						Careful.checkJumpingNotPlayed(ClientManager.ListClient[IndexClient], 0, 0, true);
						
                        playerManager.ChangeGameTurn(ClientManager.ListClient[IndexClient]);

                        ClientManager.ListClient[IndexClient].SendMsg("Temps écoulé ! Changement de tour !");
                        ClientManager.ListClient[IndexOpponent].SendMsg("Temps écoulé ! Changement de tour !");
                    }else if (TimeCount % 10 == 0 && TimeCount != 0 && TimeExist != (30 - TimeCount))
					{
						int IndexOpponent = ClientManager.byPseudo(ClientManager.ListClient[IndexClient].info_game.opponent);

						if (IndexOpponent == -1)
						{
							return;
						}

						ClientManager.ListClient[IndexClient].info_game.timeExist = 30 - TimeCount;
						ClientManager.ListClient[IndexOpponent].info_game.timeExist = 30 - TimeCount;

						ClientManager.ListClient[IndexClient].SendMsg("Il ne vous reste plus que " + (30 - TimeCount) + " secondes !");
						ClientManager.ListClient[IndexOpponent].SendMsg("Il ne lui reste plus que " + (30 - TimeCount) + " secondes !");
					}
				}
            }
        }
    }
}