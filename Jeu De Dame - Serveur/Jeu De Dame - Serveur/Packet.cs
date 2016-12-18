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

            if (firstRcv)
            {
                if (packetSpace[0] == "newClient" &&
                    (More.s_int(packetSpace[2]) == 0 || More.s_int(packetSpace[2]) == 1) &&
                    packetSpace.Length == 4)
                {
                    string pseudoByPacket = packetSpace[1];
                    int playerTopByPacket = More.s_int(packetSpace[2]);
                    string opponentByPacket = packetSpace[3];

                    if (ClientManager.byPseudo(pseudoByPacket) == -1) // Si le pseudo n'existe pas
                    {
                        Client newClient = new Client(pseudoByPacket, playerTopByPacket, opponentByPacket);

                        newClient.MySocket = ClientSocket;

                        Thread newThread = new Thread(newClient.Handler);
                        newClient.MyThread = newThread;

                        newClient.info_game.isplaying = false;
                        newClient.info_game.pawnAlive = 15;
                        newClient.info_game.tour = Convert.ToBoolean(playerTopByPacket);
                        newClient.info_game.plateauCases = new Plateau.cases[10][];

                        ClientManager.ListClient.Add(newClient);
                        newThread.Start();

                        Console.WriteLine("Client ajoute a la liste");
                        return true;
                    }
                }
                return false;
            }
            
            if (packetSpace[0] == "select" && More.isDec(packetSpace[1]) && More.isDec(packetSpace[2]) &&
                More.isDec(packetSpace[3]) && More.isDec(packetSpace[4]) && packetSpace.Length == 5)
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
            }
            
            if (packetSpace[0] == "req_result" && More.isDec(packetSpace[1]) && More.isDec(packetSpace[2]) &&
                More.isDec(packetSpace[3]) && packetSpace.Length == 4)
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
                }else
                {
                    isPlaying.Send("msg Vous avez pris trop de temps à répondre");
                }
            }

            return false;
        }
    }
}