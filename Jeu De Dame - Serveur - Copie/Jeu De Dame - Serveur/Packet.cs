using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace Jeu_De_Dame___Serveur
{
    class Packet
    {
        public static bool Handler(Client isPlaying, string packet, bool firstRcv = false, Socket ClientSocket = null)
        {
            string[] packetSpace = packet.Split(' ');

            if (packetSpace.Length < 1)
            {
                return false;
            }
			
            if (firstRcv)
            {
                if (packetSpace[0] == "addClient" && packetSpace.Length == 3)
                {
                    string pseudoByPacket = packetSpace[1];
					string passeByPacket = packetSpace[2];
                    if (ClientManager.byPseudo(pseudoByPacket) == -1) // Si le pseudo n'existe pas
                    {
                        Client newClient = new Client(pseudoByPacket, passeByPacket);

                        newClient.MySocket = ClientSocket;

                        Thread newThread = new Thread(newClient.Handler);
                        newClient.MyThread = newThread;

                        newClient.info_game.isplaying = false;

                        ClientManager.ListClient.Add(newClient);

						newClient.InitialisationConnexion(true);

						/* Thread */
						newClient.MySendThread = new Thread(newClient.ThreadSendVoid);

						newClient.MySendThread.Start();
                        newThread.Start();
						
                        return true;
                    }
                }
                return false;
            }
			Console.WriteLine("Packet recu par " + isPlaying.info_main.pseudo + " = " + packet);

			bool playing = isPlaying.info_game.isplaying;

            if (packetSpace[0] == "ok" && packetSpace.Length == 1)
            {
                isPlaying.info_main.received = true;
				return true;
            }
			
			if (packetSpace[0] == "inscription" && packetSpace.Length == 1 && !playing)
			{
				MatchMaking.InscriptionMatch(isPlaying);
				
			}

            if (packetSpace[0] == "select" && packetSpace.Length == 5 && playing)
            {
                if (More.isDec(packetSpace[1]) && More.isDec(packetSpace[2]) &&
                More.isDec(packetSpace[3]) && More.isDec(packetSpace[4]))
                {
                    int x = More.s_int(packetSpace[1]);
                    int y = More.s_int(packetSpace[2]);
                    int xSelected = More.s_int(packetSpace[3]);
                    int ySelected = More.s_int(packetSpace[4]);

                    if (x >= 0 && x <= 9 && y >= 0 && y <= 9 &&
                        xSelected >= 0 && xSelected <= 9 && ySelected >= 0 && ySelected <= 9)
                    {
                        Action.pawnMoving(isPlaying, x, y, xSelected, ySelected);
                    }
                    return true;
                }
            }

            if (packetSpace[0] == "req_result" && packetSpace.Length == 4 && playing)
            {
                if (More.isDec(packetSpace[1]) && More.isDec(packetSpace[2]) &&
                More.isDec(packetSpace[3]))
                {
                    if (isPlaying.info_game.asked)
                    {
                        int result = More.s_int(packetSpace[1]);
                        int x = More.s_int(packetSpace[2]);
                        int y = More.s_int(packetSpace[3]);

                        if (result == 1)
                        {
                            Action.setCase(isPlaying, x, y);
                        }
                        isPlaying.info_game.asked = false;
                    }
                    else
                    {
                        isPlaying.Send("msg Vous avez pris trop de temps à répondre");
                    }
                    return true;
                }
            }
            if (packetSpace[0] == "newMatch" && packetSpace.Length == 4 && !playing)
            {
                if (More.s_int(packetSpace[2]) == 0 || More.s_int(packetSpace[2]) == 1)
                {
					/*if (!isPlaying.info_main.iswait)
				   {
					  string pseudoByPacket = packetSpace[1];
					   int playerTopByPacket = More.s_int(packetSpace[2]);
					   string opponentByPacket = packetSpace[3];

					   // Traitement du match
					   int IndexClient = ClientManager.byPseudo(pseudoByPacket);

					   if (IndexClient == -1)
					   {
						   return false;
					   }

					   isPlaying.info_main.iswait = true;

					   isPlaying.info_game.opponent = opponentByPacket;
					   isPlaying.info_main.playerTop = Convert.ToBoolean(playerTopByPacket);
					   isPlaying.info_game.tour = Convert.ToBoolean(playerTopByPacket);

					   string PseudoClient = ClientManager.ListClient[IndexClient].info_main.pseudo;
					   string PseudoOpponent = ClientManager.ListClient[IndexClient].info_game.opponent;

					   Console.WriteLine("Nouveau match : " + PseudoClient + " Vs : " + PseudoOpponent);

					   int IndexOpponent = ClientManager.byPseudo(PseudoOpponent);

					   if (IndexOpponent != -1)
					   {
						   if (Match.startGame(IndexClient, IndexOpponent))
						   {
							   Console.WriteLine("Les deux joueurs sont prets ...");
							   return true;
						   }
					   }

					Console.WriteLine("L'autre joueur n'est pas encore pret ...");
                        return true;
                    }*/
				}
			}
            return false;
        }
    }
}