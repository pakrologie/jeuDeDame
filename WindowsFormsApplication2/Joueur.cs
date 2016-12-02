using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    class Joueur
    {
        public struct Infos
        {
            public bool gameTour;
            public short pawnAlive;
            public short bigPawnAlive;
            public bool playerTop;
        }
        public Infos infos;
    }
}
