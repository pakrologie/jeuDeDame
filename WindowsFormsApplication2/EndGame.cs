using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class EndGame
    {
        public static bool canMakeAnAction(Joueur Player)
        {
            bool playerTop = Player.infos.playerTop;

            for (int y = 0; y < Plateau.plateauCases.Length; y++)
            {
                for (int x = 0; x < Plateau.plateauCases[y].Length; x++)
                {
                    if (Plateau.plateauCases[y][x].pawnTop == playerTop &&
                        Plateau.plateauCases[y][x].pawnExist)
                    {
                        bool isKing = Plateau.plateauCases[y][x].king;

                        if (!isKing)
                        {
                            if (Attack.detectCanAtk(Player, x, y))
                            {

                                return true;
                            }
                        }
                        if (isKing)
                        {
                            if (Attack.detectCanAtkForKing(Player, x, y))
                            {

                                return true;
                            }
                        }

                        if (!isKing)
                        {
                            int addX = 1;
                            int addY = 1;
                            if (playerTop)
                            {
                                if (y + addY <= 9 &&
                                    x + addX <= 9 &&
                                    x + (addX * -1) >= 0)
                                {
                                    if (!Plateau.plateauCases[y + addY][x + addX].pawnExist)
                                    {
                                        return true;
                                    }
                                    if (!Plateau.plateauCases[y + addY][x + addX * -1].pawnExist)
                                    {
                                        return true;
                                    }
                                }
                            }
                            else
                            {
                                addX = -1;
                                addY = -1;
                                if (y + addY >= 0 &&
                                    x + addX >= 0 &&
                                    x + (addX * -1) <= 9)
                                {
                                    if (!Plateau.plateauCases[y + addY][x + addX].pawnExist)
                                    {
                                        return true;
                                    }
                                    if (!Plateau.plateauCases[y + addY][x + addX * -1].pawnExist)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }



                    }
                }
            }
            return false;
        }

        public static bool OpponentIsDead(Joueur Player)
        {
            if (Player.infos.pawnAlive <= 0)
            {
                MessageBox.Show("Partie terminée !");
                return true;
            }
            return false;
        }
    }
}
