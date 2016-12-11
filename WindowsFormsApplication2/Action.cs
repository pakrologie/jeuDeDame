using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication2
{
    class Action
    {
        static Panel panelMain;

        public Action(Panel _panelMain)
        {
            panelMain = _panelMain;
        }

        public static bool pawnMoving(int x, int y, int xSelected, int ySelected)
        {
            Joueur Player = playerManager.WhosNext();
            Joueur Opponent = playerManager.GetOpponent(Player);

            bool playerTop = Player.infos.playerTop;

            if (Plateau.plateauCases[y][x].pawnExist)
            {
                return false;
            }

            if (x == xSelected && y == ySelected)
            {
                MessageBox.Show("Choix annulé");
                return false;
            }
            else if (!Plateau.plateauCases[y][x].pawnExist)
            {
                int ruleDistance = Distance.distanceStatus(y, x, ySelected, xSelected, playerTop);
                
                if (ruleDistance == 0 || (ruleDistance != 2 && Plateau.plateauCases[ySelected][xSelected].mainCombo &&
                    Player.infos.iscombo) || !Distance.sameDiagonal(y, x, ySelected, xSelected))
                {
                    MessageBox.Show("Vous ne pouvez pas faire cela");
                    return false;
                }
                
                Animation.makeTransition((int)BunifuAnimatorNS.AnimationType.Transparent, x, y);
                
                Careful.resetNotCarefulOpponent(Opponent);
              
                Careful.checkJumpingNotPlayed(Player, x, y);
                
                Plateau.plateauCases[y][x].king = Plateau.plateauCases[ySelected][xSelected].king;
                Plateau.plateauCases[ySelected][xSelected].king = false;

                setCase(x, y, Plateau.plateauCases[ySelected][xSelected].pb.Image, true, playerTop);
                setCase(xSelected, ySelected);

                ImagesManager.pawnToKing(playerTop, x, y);
                
                if (ruleDistance == 2)
                {
                    bool isKing = Plateau.plateauCases[y][x].king;

                    int xPawn = Distance.xPawn;
                    int yPawn = Distance.yPawn;
                    
                    Animation.makeTransition((int)BunifuAnimatorNS.AnimationType.Particles, xPawn, yPawn);

                    setCase(xPawn, yPawn);
                    
                    Opponent.infos.pawnAlive--;
                    
                    if (!isKing)
                    {
                        if (Attack.detectCanAtk(Player, x, y))
                        {
                            Player.infos.iscombo = true;
                            Plateau.plateauCases[y][x].mainCombo = true;
                            return true;
                        }
                    }
                    else
                    {
                        if (Attack.detectCanAtkForKing(Player, x, y))
                        {
                            Player.infos.iscombo = true;
                            Plateau.plateauCases[y][x].mainCombo = true;
                            return true;
                        }
                    }
                    
                    Player.infos.iscombo = false;
                    Plateau.plateauCases[y][x].mainCombo = false;
                    
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

        public static void setCase(int x, int y, Image imgPawn = null, bool isExist = false, bool isTop = false)
        {
            try
            {
                Plateau.plateauCases[y][x].pb.Image = imgPawn;
                Plateau.plateauCases[y][x].pawnExist = isExist;
                Plateau.plateauCases[y][x].pawnTop = isTop;
            }
            catch (Exception ex)
            { }
        }

      
    }
}
