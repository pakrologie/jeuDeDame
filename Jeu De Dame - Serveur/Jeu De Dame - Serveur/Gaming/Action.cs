using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BunifuAnimatorNS;

namespace Jeu_De_Dame___Serveur
{
    class Action
    {

        public static bool pawnMoving(Client isPlaying, int x, int y, int xSelected, int ySelected)
        {
            if (isPlaying.info_game.asked)
            {
                isPlaying.SendMsg("En attente de la réponse ...");
                return false;
            }

            if (isPlaying.info_main.playerTop != isPlaying.info_game.plateauCases[ySelected][xSelected].pawnTop)
            {
                Careful.isNotCareful(isPlaying, ySelected, xSelected);
                return false;
            }

            if (!isPlaying.info_game.tour)
            {
                isPlaying.SendMsg("Ce n'est pas votre tour !");
                return false;
            }

            if (isPlaying.info_game.iscombo)
            {
                if (!isPlaying.info_game.plateauCases[ySelected][xSelected].mainCombo)
                {
                    isPlaying.SendMsg("Vous ne pouvez jouer que le pion 'combo'"); 
                    return false;
                }
            }
            
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);

            Client Player = playerManager.WhosNext(isPlaying);
            Client Opponent = playerManager.GetOpponent(Player);
          
            bool playerTop = Player.info_main.playerTop;
           
            if (ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnExist)
            {
                return false;
            }
  
            if (x == xSelected && y == ySelected)
            {
                isPlaying.SendMsg("Choix annulé");
                return false;
            }
            else if (!ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnExist)
            {
                int ruleDistance = Distance.distanceStatus(isPlaying, y, x, ySelected, xSelected, playerTop);

                if (ruleDistance == 0 || (ruleDistance != 2 && ClientManager.ListClient[IndexClient].info_game.plateauCases[ySelected][xSelected].mainCombo &&
                    Player.info_game.iscombo) || !Distance.sameDiagonal(y, x, ySelected, xSelected))
                {
                    isPlaying.SendMsg("Vous ne pouvez pas faire cela");
                    return false;
                }

                sendAnimation(isPlaying, (int)BunifuAnimatorNS.AnimationType.Transparent, x, y);

                Careful.resetNotCarefulOpponent(Opponent);

                Careful.checkJumpingNotPlayed(Player, x, y);

                ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].king = ClientManager.ListClient[IndexClient].info_game.plateauCases[ySelected][xSelected].king;
                ClientManager.ListClient[IndexClient].info_game.plateauCases[ySelected][xSelected].king = false;
                
                Match.SynchroWithOpponents(isPlaying);
                
                setCase(isPlaying, x, y, imageManager.getPawnImgByPlayer(playerTop, ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].king), true, playerTop);
                
                setCase(isPlaying, xSelected, ySelected);
             
                imageManager.pawnToKing(isPlaying, playerTop, x, y);
               
                if (ruleDistance == 2)
                {
                    bool isKing = ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].king;

                    int xPawn = Distance.xPawn;
                    int yPawn = Distance.yPawn;

                    sendAnimation(isPlaying, (int)BunifuAnimatorNS.AnimationType.Particles, xPawn, yPawn);

                    setCase(isPlaying, xPawn, yPawn);

                    Opponent.info_game.pawnAlive--;

                    if (!isKing)
                    {
                        if (Attack.detectCanAtk(Player, x, y))
                        {
                            Player.info_game.iscombo = true;
                            ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].mainCombo = true;
                            Match.SynchroWithOpponents(isPlaying);
                            return true;
                        }
                    }
                    else
                    {
                        if (Attack.detectCanAtkForKing(Player, x, y))
                        {
                            Player.info_game.iscombo = true;
                            ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].mainCombo = true;
                            Match.SynchroWithOpponents(isPlaying);
                            return true;
                        }
                    }

                    Player.info_game.iscombo = false;
                    ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].mainCombo = false;
                    Match.SynchroWithOpponents(isPlaying);

                    if (EndGame.OpponentIsDead(Opponent))
                    {
                        return true;
                    }
                }

                playerManager.ChangeGameTurn(Player);

                xSelected = -1;
                ySelected = -1;
            }
            return true;
        }
        
        public static void sendAnimation(Client isPlaying, int type, int x, int y)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);
            int IndexOpponent = ClientManager.byPseudo(isPlaying.info_game.opponent);

            if (IndexClient == -1|| IndexOpponent == -1)
            {
                return;
            }

            string packet = "anim " + type + " " + x + " " + y;

            ClientManager.ListClient[IndexClient].Send(packet);
            ClientManager.ListClient[IndexOpponent].Send(packet);
        }

        public static void setCase(Client isPlaying, int x, int y, string imgPawn = "-1", bool isExist = false, bool isTop = false)
        {
            int IndexClient = ClientManager.byPseudo(isPlaying.info_main.pseudo);
            int IndexOpponent = ClientManager.byPseudo(isPlaying.info_game.opponent);

            if (IndexClient == -1 || IndexOpponent == -1)
            {
                return;
            }

            try
            {
                string pathImg = imgPawn.ToString();

                ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnExist = isExist;

                ClientManager.ListClient[IndexClient].info_game.plateauCases[y][x].pawnTop = isTop;

                Match.SynchroWithOpponents(isPlaying);

                string packet = "set " + x + " " + y + " " + Convert.ToInt32(isExist) + " " + Convert.ToInt32(isTop) + " " + imgPawn;
                ClientManager.ListClient[IndexClient].Send(packet);
                ClientManager.ListClient[IndexOpponent].Send(packet);

            }
            catch (Exception ex)
            { }
        }
    }
}
