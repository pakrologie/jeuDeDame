using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace WindowsFormsApplication2
{
    class Client
    {
        static Socket MySocket;
        static ListBox gameList;
        static Panel gamePanel;

        public Client(ListBox _gameList, Panel _gamePanel)
        {
            gameList = _gameList;
            gamePanel = _gamePanel;
        }

        public static void connectServer(string ip, int port)
        {
            MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                MySocket.Connect("127.0.0.1", 8080);
                Treatment();
            }
            catch (Exception ex)
            { MessageBox.Show("Impossible de se connecter"); }

        }
        
        public static void Treatment()
        {
            SendPacket("newClient Zayd 1 Bilal");

            Thread Thr = new Thread(ReceivePacket);
            Thr.Start();
        }

        public static void ReceivePacket()
        {
            while (MySocket.Connected)
            {
                int packetlength = 0;
                byte[] buffer = new byte[1024];
                string packet = "";

                try
                {
                    packetlength = MySocket.Receive(buffer);
                }catch(Exception ex)
                {
                    Application.Exit();
                }

                packet = System.Text.Encoding.UTF8.GetString(buffer).Substring(0, packetlength);

                Handler(packet);
            }
        }

        public static void SendPacket(string packet)
        {
            MySocket.Send(System.Text.Encoding.UTF8.GetBytes(packet));
        }

        public static void Handler(string packet)
        {
            string[] packetSpace = packet.Split(' ');

            if (packetSpace[0] == "match" && packetSpace.Length == 2)
            {
                if (gamePanel.InvokeRequired)
                {
                    gamePanel.Invoke(new changeEnabledPanel(changeEnabledPanel_));
                }

                if (gameList.InvokeRequired)
                {
                    gameList.Invoke(new addItemsOnGameList(addItemsOnGameList_), ("C'est parti ! Contre : " + packetSpace[1]));
                }
            }

            if (packetSpace[0] == "set" && More.isDec(packetSpace[1]) && More.isDec(packetSpace[2]) &&
                More.isDec(packetSpace[3]) && More.isDec(packetSpace[4]) && packetSpace.Length == 6)
            {
                int x = More.s_int(packetSpace[1]);
                int y = More.s_int(packetSpace[2]);
                int isExist = More.s_int(packetSpace[3]);
                int isTop = More.s_int(packetSpace[4]);
                string imgPawn = packetSpace[5];
                Action.setCase(x, y, imgPawn, Convert.ToBoolean(isExist), Convert.ToBoolean(isTop));
            }

            if (packetSpace[0] == "anim" && More.isDec(packetSpace[1]) && More.isDec(packetSpace[2]) &&
                More.isDec(packetSpace[3]) && packetSpace.Length == 4)
            {
                int type = More.s_int(packetSpace[1]);
                int x = More.s_int(packetSpace[2]);
                int y = More.s_int(packetSpace[3]);

                Animation.makeTransition(type, x, y);
            }
            
            if (packetSpace[0] == "msg" && packetSpace.Length >= 2)
            {
                string message = "";
                for (int i = 1; i < packetSpace.Length; i++)
                {
                    message += packetSpace[i];
                    message += " ";
                }

                if (gameList.InvokeRequired)
                {
                    gameList.Invoke(new addItemsOnGameList(addItemsOnGameList_), message);
                }
            }

            if (packetSpace[0] == "msg_final" && packetSpace.Length >= 2)
            {
                string message = "";
                for (int i = 1; i < packetSpace.Length; i++)
                {
                    message += packetSpace[i];
                    message += " ";
                }

                MessageBox.Show(message);

                Application.Exit();
            }

            if (packetSpace[0] == "req" && More.isDec(packetSpace[1]) && More.isDec(packetSpace[2]) && More.isDec(packetSpace[3])
                && More.isDec(packetSpace[4]) && More.isDec(packetSpace[5]) && packetSpace.Length >= 7)
            {
                int type = More.s_int(packetSpace[1]);
                int bt = More.s_int(packetSpace[2]);
                int ic = More.s_int(packetSpace[3]);
                int x = More.s_int(packetSpace[4]);
                int y = More.s_int(packetSpace[5]);

                string message = "";
                for (int i = 6; i < packetSpace.Length; i++)
                {
                    message += packetSpace[i];
                    message += " ";
                }

                DialogResult result = MessageBox.Show(message, "", (MessageBoxButtons)bt, (MessageBoxIcon)ic);

                if (result == DialogResult.Yes)
                {
                    string reqResult = "req_result 1 " + x + " " + y;
                    SendPacket(reqResult);
                }else
                {
                    SendPacket("req_result 0 -1 -1");
                }
            }
            if (packetSpace[0] == "end" && packetSpace.Length == 1)
            {
                // Fermeture du client
                Application.Exit();
            }

        }

        delegate void changeEnabledPanel();
        public static void changeEnabledPanel_()
        {
            gamePanel.Enabled = true;
        }

        delegate void addItemsOnGameList(string message);
        public static void addItemsOnGameList_(string message)
        {
            gameList.Items.Add(message);
        }
    }
}
