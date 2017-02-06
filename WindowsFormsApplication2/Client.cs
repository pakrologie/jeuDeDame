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
        
        static Panel gamePanel;
        static ListBox gameList;
        static Form MyGameForm;
        static Form MyMainUIForm;

        public Client(Panel _gamePanel, ListBox _gameList, Form _MyGameForm, Form _MyMainUIForm)
        {
            gamePanel = _gamePanel;
            gameList = _gameList;
            MyGameForm = _MyGameForm;
            MyMainUIForm = _MyMainUIForm;
        }

        public static void connectServer(string ip, int port, string userName)
        {
            MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                MySocket.Connect(ip, port);
                Treatment(userName);
            }
            catch (Exception ex)
            { MessageBox.Show("Impossible de se connecter"); Environment.Exit(1); }

        }
        
        public static void Treatment(string userName)
        {
            SendPacket("addClient " + userName);

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

                /*gameList.Invoke((MethodInvoker)delegate ()
                {
                    string message = "packet recu : " + packet;
                    gameList.Items.Add(message);
                    gameList.SelectedIndex = gameList.Items.Count - 1;
                });*/

                Handler(packet);
            }
        }

        public static void SendPacket(string packet)
        {
            MySocket.Send(System.Text.Encoding.UTF8.GetBytes(packet));
        }

        public static void Handler(string packet)
        {
            try
            {
                string[] packetSpace = packet.Split(' ');

                if (packetSpace[0] == "match" && packetSpace.Length == 2)
                {
                    MyMainUIForm.Hide();
                    MyGameForm.Invoke((MethodInvoker)delegate () {
                        MyGameForm.Show();
                    });

                    gameList.Invoke((MethodInvoker)delegate ()
                    {
                        string message = "C'est parti ! Contre : " + packetSpace[1];
                        gameList.Items.Add(message);
                        gameList.SelectedIndex = gameList.Items.Count - 1;
                    });

                    gamePanel.Invoke((MethodInvoker)delegate ()
                    {
                        gamePanel.Enabled = true;
                    });
                }
                if (packetSpace[0] == "set" && packetSpace.Length == 6)
                {
                    if (More.isDec(packetSpace[1]) && More.isDec(packetSpace[2]) &&
                    More.isDec(packetSpace[3]) && More.isDec(packetSpace[4]))
                    {
                        int x = More.s_int(packetSpace[1]);
                        int y = More.s_int(packetSpace[2]);
                        int isExist = More.s_int(packetSpace[3]);
                        int isTop = More.s_int(packetSpace[4]);
                        string imgPawn = packetSpace[5];
                        Action.setCase(x, y, imgPawn, Convert.ToBoolean(isExist), Convert.ToBoolean(isTop));
                    }
                }

                if (packetSpace[0] == "anime" && packetSpace.Length == 4) // todo : anime -> anim
                {
                    if (More.isDec(packetSpace[1]) && More.isDec(packetSpace[2]) &&
                    More.isDec(packetSpace[3]))
                    {
                        int type = More.s_int(packetSpace[1]);
                        int x = More.s_int(packetSpace[2]);
                        int y = More.s_int(packetSpace[3]);

                        /* Test */
                        Thread AnimThr = new Thread(() => Animation.makeTransition(type, x, y));
                        AnimThr.Start();

                        //Animation.makeTransition(type, x, y);
                    }
                }

                if (packetSpace[0] == "msg" && packetSpace.Length >= 2)
                {
                    string message = "";
                    for (int i = 1; i < packetSpace.Length; i++)
                    {
                        message += packetSpace[i];
                        message += " ";
                    }

                    gameList.Invoke((MethodInvoker)delegate ()
                    {
                        gameList.Items.Add(message);
                        gameList.SelectedIndex = gameList.Items.Count - 1;
                    });
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

                     CloseGameOpenUI();
                }

                if (packetSpace[0] == "req" && packetSpace.Length >= 7)
                {
                    if (More.isDec(packetSpace[1]) && More.isDec(packetSpace[2]) && More.isDec(packetSpace[3])
                    && More.isDec(packetSpace[4]) && More.isDec(packetSpace[5]))
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
                            Client.SendPacket(reqResult);
                        }
                        else
                        {
                            Client.SendPacket("req_result 0 -1 -1");
                        }
                    }
                }
                if (packetSpace[0] == "end" && packetSpace.Length == 1)
                {
                    CloseGameOpenUI();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void CloseGameOpenUI()
        {
            MyMainUIForm.Invoke((MethodInvoker)delegate () {
                MyMainUIForm.Show();
            });
            
            ImagesManager MyImageM = new ImagesManager(gamePanel);

            MyImageM.resetPlateau();
            gameList.Items.Clear();
            
            MyGameForm.Invoke((MethodInvoker)delegate () {
                MyGameForm.Hide();
            });
        }

    }
}
