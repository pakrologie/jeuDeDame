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
                // Récupère la distance entre la position initiale et finale
                int ruleDistance = Distance.distanceOk(y, x, ySelected, xSelected, Player.infos.playerTop);

                // Si la distance n'est pas respecté et que votre pion n'est pas en Etat ' King '
                if (ruleDistance == 0 && !Plateau.plateauCases[y][x].king)
                {
                    MessageBox.Show("Vous ne pouvez pas faire cela");
                    return false;
                }

                // Affichage d'une animation lors du déplacement
                Animation.makeTransition((int)BunifuAnimatorNS.AnimationType.Transparent, x, y);

                // Mise à jour de l'interface
                setCase(x, y, ImagesManager.getPawnImgByPlayer(Player), true, Player.infos.playerTop);
                setCase(xSelected, ySelected);
                
                // Si le joueur a décidé d'attaquer un pion adverse
                if (ruleDistance == 2)
                {
                    // On récupère les coordonnées du pion attaqué
                    int xPawn = Distance.xPawn;
                    int yPawn = Distance.yPawn;

                    // Affichage d'une animation lors de la destruction du pion
                    Animation.makeTransition((int)BunifuAnimatorNS.AnimationType.Particles, xPawn, yPawn);

                    setCase(xPawn, yPawn);

                    // Mise à jour des pions adverses [ 0 = Adversaire à perdu ]
                    Opponent.infos.pawnAlive--;
                    
                    // Détecte si vous n'avez pas attaquer un pion lorsque vous en avez eu l'occasion
                    if (RuleAdvanced.detectCanEat(Player, x, y))
                    {
                        Player.infos.iscombo = true;
                        Plateau.plateauCases[y][x].mainCombo = true;
                        return true;
                    }
                    else // Met à jour les variables 'combo' du Joueur 
                    {
                        Player.infos.iscombo = false;
                        Plateau.plateauCases[y][x].mainCombo = false;
                    }
                }
                
                // Met à jour les variables ' combo ' de l'adversaire
                RuleAdvanced.resetNotCarefulOpponent(Opponent);
                // Met à jour la variable 'isNotCareful' aux pions n'ayant pas attaquer l'adversaire lorsqu'il en a eu l'occasion
                Rule.checkJumpingNotPlayed(Player, x, y);

                // Intervertit les tours des joueurs
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
