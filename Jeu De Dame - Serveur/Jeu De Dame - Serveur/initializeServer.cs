using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Jeu_De_Dame___Serveur
{
    class initializeServer
    {
        public static void start(string ip, int port)
        {
            Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            ServerSocket.Listen(1);

            Console.WriteLine("Le serveur est pret");

            while (true)
            {
                Socket SocketReceived = ServerSocket.Accept();
                ClientHandler(SocketReceived);
            }
        }

        public static void ClientHandler(Socket Client)
        {
            Console.WriteLine("nouveau client recu");

            int packetlength = 0;
            byte[] buffer = new byte[1024];
            string packet;

            try
            {
                packetlength = Client.Receive(buffer);
            }catch(Exception ex)
            {
                goto outOfTreatment;
            }

            packet = More.b_str(buffer, packetlength);

            if (Packet.Handler(null, packet, true, Client))
            {
                Console.WriteLine("Nouveau client ajouté !");
                return;
            }

            outOfTreatment:
            Console.WriteLine("Le client a ete perdu");
        }
    }
}
