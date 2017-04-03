using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeu_De_Dame___Serveur
{
    class Attack
    {
        public static Point GetCoordsAttakedOpponent(Client isPlaying, int y1, int x1, int y2, int x2, bool playerTop)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);

            if (IndexClient == -1)
            {
                return new Point(-1, -1);
            }

            for (int y = 0; y < ClientManager.ListClient[IndexClient].info_game.plateauCases.Length; y++)
            {
                for (int x = 0; x < ClientManager.ListClient[IndexClient].info_game.plateauCases[y].Length; x++)
                {
                    if (ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnExist)
                    {
                        int distance1 = Distance.getDistance(y, x, y1, x1);
                        int distance2 = Distance.getDistance(y, x, y2, x2);

                        if (distance1 == 1 && distance2 == 1)
                        {
                            if (playerTop != ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnTop)
                            {
                                return new Point(x, y);
                            }
                        }
                    }
                }
            }
            return new Point(-1, -1);
        }

        public static bool detectCanAtk(Client isPlaying, int x, int y)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);

            if (IndexClient == -1)
            {
                return false;
            }

            bool playerTop = isPlaying.info_main.playerTop;

            for (int y1 = 0; y1 < ClientManager.ListClient[IndexClient].info_game.plateauCases.Length; y1++)
            {
                for (int x1 = 0; x1 < ClientManager.ListClient[IndexClient].info_game.plateauCases[y1].Length; x1++)
                {
                    if (ClientManager.ListClient[IndexClient].info_game.plateauCases[y1][x1].pawnTop != playerTop &&
                        ClientManager.ListClient[IndexClient].info_game.plateauCases[y1][x1].pawnExist)
                    {
                        int distance = Distance.getDistance(y, x, y1, x1);
                        if (distance == 1)
                        {
                            Point coeffCoords = Maths.getCoeffDiff(x1, y1, x, y);
                            int countDiffY = coeffCoords.Y * 2;
                            int countDiffX = coeffCoords.X * 2;

                            if ((x + countDiffX) <= 9 && (y + countDiffY) <= 9 &&
                                (x + countDiffX) >= 0 && (y + countDiffY) >= 0)
                            {
                                if (!ClientManager.ListClient[IndexClient].info_game.plateauCases[y + countDiffY][x + countDiffX].pawnExist)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool detectCanAtkForKing(Client isPlaying, int x, int y)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);

            if (IndexClient == -1)
            {
                return false;
            }

            bool playerTop = isPlaying.info_main.playerTop;

            for (int y1 = 0; y1 < ClientManager.ListClient[IndexClient].info_game.plateauCases.Length; y1++)
            {
                for (int x1 = 0; x1 < ClientManager.ListClient[IndexClient].info_game.plateauCases[y1].Length; x1++)
                {
                    if (ClientManager.ListClient[IndexClient].info_game.plateauCases[y1][x1].pawnTop != playerTop &&
                        ClientManager.ListClient[IndexClient].info_game.plateauCases[y1][x1].pawnExist)
                    {
                        Point coeffCoords = Maths.getCoeffDiff(x1, y1, x, y);

                        int addCoeffX = coeffCoords.X;
                        int addCoeffY = coeffCoords.Y;

                        int xData = x1 + addCoeffX;
                        int yData = y1 + addCoeffY;

                        if (xData <= 9 && yData <= 9 &&
                            xData >= 0 && yData >= 0)
                        {
                            if (Distance.freeField(isPlaying, playerTop, xData, yData, x, y, false) == 2)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}