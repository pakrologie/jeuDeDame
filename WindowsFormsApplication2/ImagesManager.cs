﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication2
{
    class ImagesManager
    {
        static Form formMain;

        int xSelected = -1;
        int ySelected = -1;

        public ImagesManager(Form _formMain)
        {
            formMain = _formMain;
        }

        private void pawnDown(object sender, MouseEventArgs e)
        {

            PictureBox pawn = (PictureBox)sender;

            xSelected = pawn.Location.X / 50;
            ySelected = pawn.Location.Y / 50;

        }

        private void pawnUp(object sender, MouseEventArgs e)
        {
            int xMouse = Cursor.Position.X / 50;
            int yMouse = Cursor.Position.Y / 50;

            PictureBox pawn = (PictureBox)sender;

            int x = pawn.Location.X / 50;
            int y = pawn.Location.Y / 50;
            //pawnMoving(x, y);
            MessageBox.Show("x = " + xMouse + " - y = " + yMouse);
           // MessageBox.Show("xSe = " + xSelected + " - ySe = " + ySelected);
        }

        public void createPictureBox()
        {
            for (int y = 0; y < Plateau.countHorizontal; y++)
            {
                for (int x = 0; x < Plateau.countVertical; x++)
                {
                    if (Plateau.plateauCases[y][x].isBlack)
                    {
                        PictureBox pb = new PictureBox();

                        int locaX = Plateau.plateauCases[y][x].Rec.X;
                        int locaY = Plateau.plateauCases[y][x].Rec.Y;

                        pb.Location = new Point(locaX, locaY);
                        pb.Width = Plateau.plateauCases[y][x].Rec.Width;
                        pb.Height = Plateau.plateauCases[y][x].Rec.Height;
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;
                        pb.BackColor = Color.Black;
                        pb.BorderStyle = BorderStyle.None;
                        
                        pb.MouseDown += pawnDown;
                        pb.MouseUp += pawnUp;

                        Plateau.plateauCases[y][x].pb = pb;
                        Plateau.plateauCases[y][x].king = false;
                        Bitmap img = null;

                        if (y < 3) // Pion du haut
                        {
                            img = new Bitmap(@"pion_1.png");
                            Plateau.plateauCases[y][x].pawnExist = true;
                            Plateau.plateauCases[y][x].pawnTop = true;
                        }
                        if (y > 6) // Pion du bas
                        {
                            img = new Bitmap(@"pion_2.png");
                            Plateau.plateauCases[y][x].pawnExist = true;
                            Plateau.plateauCases[y][x].pawnTop = false;
                        }

                        pb.Image = img;

                        formMain.Controls.Add(pb);
                    }
                }
            }
        }

        public void pawnMoving(int x, int y)
        {
            Joueur Player = playerManager.WhosNext();
            Joueur Opponent = playerManager.GetOpponent(Player);

            if (Player.infos.playerTop != Plateau.plateauCases[y][x].pawnTop)
            {
                MessageBox.Show("Ce n'est pas vos pions !");
                return;
            }
            
            if (x == xSelected && y == ySelected) // " Second clic "
            {
                xSelected = -1;
                ySelected = -1;

                MessageBox.Show("Choix annulé");
            }
            else if (!Plateau.plateauCases[y][x].pawnExist) // " Second clic "
            {
                int ruleDistance = Rule.distanceOk(y, x, ySelected, xSelected, Player.infos.playerTop);

                if (ruleDistance == 0 && !Plateau.plateauCases[y][x].king)
                {
                    MessageBox.Show("Vous ne pouvez pas faire cela");
                    return;
                }

                // Mise à jour de l'interface
                Plateau.plateauCases[y][x].pb.Image = Plateau.plateauCases[ySelected][xSelected].pb.Image;
                Plateau.plateauCases[y][x].pawnExist = true;
                Plateau.plateauCases[y][x].pawnTop = Plateau.plateauCases[ySelected][xSelected].pawnTop;

                Plateau.plateauCases[ySelected][xSelected].pb.Image = null;
                Plateau.plateauCases[ySelected][xSelected].pawnExist = false;
                Plateau.plateauCases[ySelected][xSelected].pawnTop = false;

                if (ruleDistance == 2) // Attaque un pion adverse
                {
                    int x_pawn = Rule.x_pawn;
                    int y_pawn = Rule.y_pawn;

                    Plateau.plateauCases[y_pawn][x_pawn].pb.Image = null;
                    Plateau.plateauCases[y_pawn][x_pawn].pawnExist = false;
                    Plateau.plateauCases[y_pawn][x_pawn].pawnTop = false;
                }

                // Mise à jour des informations Joueurs
                if (playerManager.Joueur1 == Player)
                {
                    playerManager.Joueur1.infos.gameTour = false;
                    playerManager.Joueur2.infos.gameTour = true;
                }
                else
                {
                    playerManager.Joueur1.infos.gameTour = true;
                    playerManager.Joueur2.infos.gameTour = false;
                }

                if (ruleDistance == 2)
                {
                    Opponent.infos.pawnAlive--;
                    MessageBox.Show("Pion(s) restant(s) = " + Opponent.infos.pawnAlive);
                }
                
                xSelected = -1;
                ySelected = -1;
            }
    
        }
    }
}
