using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Jeu_De_Dame___Serveur
{
    class Client
    {
        public Socket MySocket;
        public Thread MyThread;

        public infos info_main;
        public game info_game;

        public Client(string pseudo)
        {
            info_main.pseudo = pseudo;
            //info_main.playerTop = Convert.ToBoolean(playerTop);
            //info_game.opponent = opponent;
        }
        
        public struct infos
        {
            public string pseudo;
            public bool playerTop;
        }

        public struct game
        {
            public string opponent;
            public bool isplaying;
            public bool tour;
            public int pawnAlive;
            public bool iscombo;
            public Plateau.cases[][] plateauCases;
            public bool asked;
            public int timeCount;
        }

        public void Deconnexion()
        {
            ClientManager.RedirectEnding(this, true);

            ClientManager.threadLock.WaitOne();

            ClientManager.ListClient.RemoveAt(ClientManager.bySocket(MySocket));

            ClientManager.threadLock.ReleaseMutex();

            Console.WriteLine("Le client " + info_main.pseudo + " vient de se deconnecter");

            if (MySocket != null)
            {
                MySocket.Close();
            }

            if (MyThread != null)
            {
                if (MyThread.IsAlive)
                {
                    MyThread.Abort();
                }
            }
        }
        
        public void Send(string packet)
        {
            Console.WriteLine(this.info_main.pseudo + " (" + packet + ") envoyé");
            MySocket.Send(System.Text.Encoding.UTF8.GetBytes(packet));

            System.Threading.Thread.Sleep(20);
        }

        public void SendMsg(string message)
        {
            string packet = "msg " + message;
            Send(packet);
        }

        public void SendRequest(string message, MessageBoxButtons bt, MessageBoxIcon ic, int x, int y) 
        {
            string packet = "req 0 " + (int)bt + " " + (int)ic + " " + x + " " + y + " " + message;
            Send(packet);
        }
        
        public void Handler()
        {
            while (MySocket.Connected)
            {
                int packetlength = 0;
                string packet;
                byte[] buffer = new byte[1024];

                try
                {
                    packetlength = MySocket.Receive(buffer);
                    packet = More.b_str(buffer, packetlength);
                    Packet.Handler(this, packet, false);
                }catch(Exception ex)
                {
                    this.Deconnexion();
                }
            }
        }
    }

    class ClientManager
    {
        public static List<Client> ListClient = new List<Client>();
        public static Mutex threadLock = new Mutex();

        public static int bySocket(Socket SocketClient)
        {
            for (int i = 0; i < ListClient.Count; i++)
            {
                if (ListClient[i].MySocket == SocketClient)
                {
                    return i;
                }
            }

            return -1;
        }

        public static int byPseudo(string PseudoClient)
        {
            for (int i = 0; i < ListClient.Count; i++)
            {
                if (ListClient[i].info_main.pseudo == PseudoClient)
                {
                    return i;
                }
            }

            return -1;
        }

        public static void RedirectEnding(Client PlayerDefeat, bool forfeit)
        {
            int IndexOpponent = byPseudo(PlayerDefeat.info_game.opponent);
            
            if (IndexOpponent == -1)
            {
                return;
            }

            PlayerDefeat.info_game.isplaying = false;
            ListClient[IndexOpponent].info_game.isplaying = false;

            Victory(ListClient[IndexOpponent], forfeit);
            Defeat(PlayerDefeat, forfeit);
        }

        public static void Victory(Client PlayerVictory, bool forfeit)
        {
            if (forfeit)
            {
                PlayerVictory.Send("msg_final L'adversaire s'est déconnecté ... Vous avez donc gagné !");
            }
            else
            {
                PlayerVictory.Send("msg_final Félicitation! Vous avez réussi à battre votre adversaire, vous avez gagné !");
            }
        }

        public static void Defeat(Client PlayerDefeat, bool forfeit)
        {
            if (!forfeit)
            {
                PlayerDefeat.Send("msg_final Vous avez perdu !");
            }
        }
    }
}
