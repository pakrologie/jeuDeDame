using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Jeu_De_Dame___Serveur
{
    class Program
    {
        public static Thread CheckTimeThr;
        static void Main(string[] args)
        {
            CheckTimeThr = new Thread(CheckTime.CheckDuration);
            CheckTimeThr.Start();

            initializeServer.start("25.76.174.33", 8080);
        }
    }
}
