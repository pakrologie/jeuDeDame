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
        public Thread MySendThread;

        public infos info_main;
        public game info_game;

        public List<string> PacketToSend;

        public Client(string pseudo)
        {
            info_main.pseudo = pseudo;
            info_main.received = true;
            PacketToSend = new List<string>();
        }
        
        public struct infos
        {
            public string pseudo;
            public bool playerTop;
            public bool iswait;
            public bool received;
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

        public void ThreadSendVoid()
        {
            while (MySocket.Connected)
            {
                for (int i = 0; i < PacketToSend.Count; i++)
                {
                    if (info_main.received)
                    {
                        Console.WriteLine(this.info_main.pseudo + " (" + PacketToSend[i] + ") envoyé");
                        MySocket.Send(System.Text.Encoding.UTF8.GetBytes(PacketToSend[i]));
                        info_main.received = false;
                        PacketToSend.RemoveAt(i);
                    }
                }
            }
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
            if (MySocket.Connected)
            {
                if (!String.IsNullOrWhiteSpace(packet))
                {
                    PacketToSend.Add(packet);
                }
            }
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

                System.Threading.Thread.Sleep(20);
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
            if (PlayerDefeat.info_game.isplaying)
            {
                int IndexOpponent = byPseudo(PlayerDefeat.info_game.opponent);

                if (IndexOpponent == -1)
                {
                    return;
                }

                if (ListClient[IndexOpponent].info_game.isplaying)
                {
                    PlayerDefeat.info_game.isplaying = false;
                    ListClient[IndexOpponent].info_game.isplaying = false;

                    Victory(ListClient[IndexOpponent], forfeit);
                    Defeat(PlayerDefeat, forfeit);
                }
            }
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
            PlayerVictory.info_game.isplaying = false;
            PlayerVictory.info_main.iswait = false;
        }

        public static void Defeat(Client PlayerDefeat, bool forfeit)
        {
            if (!forfeit)
            {
                PlayerDefeat.Send("msg_final Vous avez perdu !");

                PlayerDefeat.info_game.isplaying = false;
                PlayerDefeat.info_main.iswait = false;
            }
        }
    }
}
