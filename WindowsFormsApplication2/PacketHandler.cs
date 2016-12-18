using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class PacketHandler
    {
        static Panel gamePanel;
        static ListBox gameList;

        public PacketHandler(Panel _gamePanel, ListBox _gameList)
        {
            gamePanel = _gamePanel;
            gameList = _gameList;
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

            if (packetSpace[0] == "anim" && packetSpace.Length == 4)
            {
                if (More.isDec(packetSpace[1]) && More.isDec(packetSpace[2]) &&
                More.isDec(packetSpace[3]))
                {
                    int type = More.s_int(packetSpace[1]);
                    int x = More.s_int(packetSpace[2]);
                    int y = More.s_int(packetSpace[3]);

                    Animation.makeTransition(type, x, y);
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
                Application.Exit();
            }
        }
        
        delegate void openGameform();
        public static void openGameform_()
        {
            Form GameForm = new gameForm();
            Form MainInterface = new mainUI("", "");
            MainInterface.Hide();
            GameForm.Show();
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
