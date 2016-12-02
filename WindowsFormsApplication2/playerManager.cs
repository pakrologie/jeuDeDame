using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    class playerManager
    {
        public const short pawnCount = 15;
        public static Joueur Joueur1;
        public static Joueur Joueur2;
        public static void createPlayers()
        {
            Joueur1 = new Joueur();
            Joueur1.infos.pawnAlive = pawnCount;
            Joueur1.infos.gameTour = true;
            Joueur1.infos.playerTop = true;
            Joueur1.infos.bigPawnAlive = 0;

            Joueur2 = new Joueur();
            Joueur2.infos.pawnAlive = pawnCount;
            Joueur2.infos.gameTour = false;
            Joueur2.infos.playerTop = false;
            Joueur2.infos.bigPawnAlive = 0;
        }

        public static Joueur WhosNext()
        {
            if (Joueur1.infos.gameTour)
            {
                return Joueur1;
            }
            return Joueur2;
        }

        public static Joueur GetOpponent(Joueur Player)
        {
            if (Player == Joueur1)
            {
                return Joueur2;
            }
            return Joueur1;
        }
    }
}
