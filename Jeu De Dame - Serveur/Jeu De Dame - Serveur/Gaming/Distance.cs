using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeu_De_Dame___Serveur
{
    class Distance
    {
        public static int xPawn;
        public static int yPawn;

        /* 
         * 0 = Distance insuffisante
         * 1 = Déplacement d'une case
         * 2 = Mange un pion
        */

        public static int distanceStatus(Client isPlaying, int y1, int x1, int y2, int x2, bool playerTop)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);

            if (IndexClient == -1)
            {
                return 0;
            }

            bool isKing = ClientManager.ListClient[IndexClient].info_game.plateauCases[y2][x2].king;
            int distance = getDistance(y1, x1, y2, x2);

            if (distance == 3) // Position d'attaque
            {
                Point OpponentCoords = Attack.GetCoordsAttakedOpponent(isPlaying, y1, x1, y2, x2, playerTop);

                if (OpponentCoords.X == -1)
                {
                    if (!isKing)
                    {
                        return 0;
                    }
                }
                else
                {
                    yPawn = OpponentCoords.Y;
                    xPawn = OpponentCoords.X;
                    return 2;
                }
            }
            else if (distance == 1) // Position de déplacement
            {
                if (!isKing)
                {
                    if (playerTop)
                    {
                        if (y1 - y2 != 1)
                            return 0;
                    }
                    else
                     if (y2 - y1 != 1)
                        return 0;
                }

                return 1;
            }

            if (distance >= 3 && isKing) // Déplacement pour les pions ' king '
            {
                int fre = freeField(isPlaying, playerTop, x1, y1, x2, y2);
                return fre;
            }

            return 0;
        }
        /*
         * 0 = Pas le droit d'avancer
         * 1 = Champ libre
         * 2 = Pion adverse présente & possibilité de l'attaquer
        */
        public static int freeField(Client isPlaying, bool playerTop, int x1, int y1, int x2, int y2, bool attacking = true)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);

            if (IndexClient == -1)
            {
                return 0;
            }

            Point coeffCoords = Maths.getCoeffDiff(x1, y1, x2, y2);
            int addCoeffX = coeffCoords.X;
            int addCoeffY = coeffCoords.Y;

            for (int i = 1; i < Math.Abs(x1 - x2); i++)
            {
                int xA = x2 + (i * addCoeffX);
                int yA = y2 + (i * addCoeffY);

                if (xA >= 0 && xA <= 9 &&
                    yA >= 0 && yA <= 9)
                {
                    if (ClientManager.ListClient[IndexClient].info_game.plateauCases[yA][xA].pawnExist)
                    {
                        if (ClientManager.ListClient[IndexClient].info_game.plateauCases[yA][xA].pawnTop == playerTop)
                        {
                            return 0;
                        }
                        else
                        {
                            int xData = xA + addCoeffX;
                            int yData = yA + addCoeffY;

                            for (int e = 1; e < Math.Abs(xA - x1) + 1; e++)
                            {
                                xData = xA + (addCoeffX * e);
                                yData = yA + (addCoeffY * e);

                                if (ClientManager.ListClient[IndexClient].info_game.plateauCases[yData][xData].pawnExist)
                                {
                                    return 0;
                                }
                                if (xData == x1 && yData == y1)
                                {
                                    if (attacking)
                                    {

                                        xPawn = xA;
                                        yPawn = yA;
                                    }
                                    return 2;
                                }
                            }
                        }
                    }
                }

            }
            return 1;
        }

        public static bool sameDiagonal(int x1, int y1, int x2, int y2)
        {
            Point coeffCoords = Maths.getCoeffDiff(x1, y1, x2, y2);
            int addCoeffX = coeffCoords.X;
            int addCoeffY = coeffCoords.Y;

            int xData = x2;
            int yData = y2;

            for (int i = 0; i < Math.Abs(x1 - x2); i++)
            {
                xData += addCoeffX;
                yData += addCoeffY;

                if (xData == x1 && yData == y1)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isLastLine(bool playerTop, int y)
        {
            if (playerTop)
            {
                if (y == 9)
                {
                    return true;
                }
            }
            else
            {
                if (y == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static int getDistance(int y1, int x1, int y2, int x2)
        {
            return Convert.ToInt32(Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
        }
    }
}
