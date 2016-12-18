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

        public Client()
        {

        }

        public static void connectServer(string ip, int port, string userName)
        {
            MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                MySocket.Connect("127.0.0.1", 8080);
                Treatment(userName);
            }
            catch (Exception ex)
            { MessageBox.Show("Impossible de se connecter"); }

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

                PacketHandler.Handler(packet);
            }
        }

        public static void SendPacket(string packet)
        {
            MySocket.Send(System.Text.Encoding.UTF8.GetBytes(packet));
        }

    }
}
