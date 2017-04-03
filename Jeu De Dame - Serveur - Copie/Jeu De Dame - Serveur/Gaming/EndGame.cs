using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jeu_De_Dame___Serveur
{
    class EndGame
    {
        public static bool canMakeAnAction(Client isPlaying)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);

            if (IndexClient == -1)
            {
                return false;
            }

            bool playerTop = isPlaying.info_main.playerTop;

            for (int y = 0; y < ClientManager.ListClient[IndexClient].info_game.plateauCases.Length; y++)
            {
                for (int x = 0; x < ClientManager.ListClient[IndexClient].info_game.plateauCases[y].Length; x++)
                {
                    if (ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnTop == playerTop &&
                        ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnExist)
                    {
                        bool isKing = ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].king;

                        if (!isKing)
                        {
                            if (Attack.detectCanAtk(isPlaying, x, y))
                            {
                                return true;
                            }
                        }
                        if (isKing)
                        {
                            if (Attack.detectCanAtkForKing(isPlaying, x, y))
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
								if (y + addY <= 9 && x + addX <= 9)
								{
									if (!ClientManager.ListClient[IndexClient].info_game.plateauCases[y + addY][x + addX].pawnExist)
									{
										return true;
									}
								}
								if (y + addY <= 9 && x + (addX * -1) >= 0)
								{
									if (!ClientManager.ListClient[IndexClient].info_game.plateauCases[y + addY][x + addX * -1].pawnExist)
									{
										return true;
									}
								}
                            }
                            else
                            {
                                addX = -1;
                                addY = -1;

								if (y + addY >= 0 && x + addX >= 0)
								{
									if (!ClientManager.ListClient[IndexClient].info_game.plateauCases[y + addY][x + addX].pawnExist)
									{
										return true;
									}
								}
								if (y + addY >= 0 && x + (addX * -1) <= 9)
								{
									if (!ClientManager.ListClient[IndexClient].info_game.plateauCases[y + addY][x + addX * -1].pawnExist)
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

        public static bool OpponentIsDead(Client isPlaying)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);
            int IndexOpponent = ClientManager.byPseudo(isPlaying.info_game.opponent);

            if (IndexClient == -1 ||IndexOpponent == -1)
            {
                return false;
            }

            if (isPlaying.info_game.pawnAlive <= 0)
            {
                ClientManager.RedirectEnding(isPlaying, false);
                return true;
            }
            return false;
        }
    }
}
