using System;
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

        public ImagesManager(Form _formMain)
        {
            formMain = _formMain;
        }

        public void createPictureBox()
        {
            for (int x = 0; x < Plateau.countHorizontal; x++)
            {
                for (int y = 0; y < Plateau.countVertical; y++)
                {
                    if (Plateau.plateauCases[x][y].isBlack)
                    {
                        PictureBox pb = new PictureBox();

                        int locaX = Plateau.plateauCases[x][y].Rec.X;
                        int locaY = Plateau.plateauCases[x][y].Rec.Y;
                        pb.Location = new Point(locaX, locaY);
                        pb.Width = Plateau.plateauCases[x][y].Rec.Width;
                        pb.Height = Plateau.plateauCases[x][y].Rec.Height;
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;
                        pb.BackColor = Color.Black;
                        pb.BorderStyle = BorderStyle.None;
                        pb.Click += new EventHandler(pictureBoxClick);
                        Plateau.plateauCases[x][y].pb = pb;
                        Bitmap img = null;

                        if (x < 3) // Pion du haut
                        {
                            img = new Bitmap(@"pion_1.png");
                            Plateau.plateauCases[x][y].pawnExist = true;
                            Plateau.plateauCases[x][y].pawnTop = true;
                        }
                        if (x > 6) // Pion du bas
                        {
                            img = new Bitmap(@"pion_2.png");
                            Plateau.plateauCases[x][y].pawnExist = true;
                            Plateau.plateauCases[x][y].pawnTop = false;
                        }

                        pb.Image = img;

                        formMain.Controls.Add(pb);
                    }
                }
            }
        }

        int xSelected = -1;
        int ySelected = -1;
        bool pawnSelected = false;

        private void pictureBoxClick(object sender, EventArgs e)
        {
            Joueur Player = playerManager.WhosNext();

            var pictureBox = (PictureBox)sender;

            // On inverse les coordonnées
            int y = pictureBox.Location.X / 50;
            int x = pictureBox.Location.Y / 50;

            if (!pawnSelected && Plateau.plateauCases[x][y].pawnExist) // " Premier clic "
            {
                if (Player.infos.playerTop == Plateau.plateauCases[x][y].pawnTop)
                {
                    xSelected = x;
                    ySelected = y;

                    pawnSelected = true;
                }else
                    MessageBox.Show("Ce n'est pas vos pions !");
            }
            else if (pawnSelected && x == xSelected && y == ySelected) // " Second clic "
            {
                pawnSelected = false;
                xSelected = -1;
                ySelected = -1;

                MessageBox.Show("Choix annulé");
            }
            else if (pawnSelected && !Plateau.plateauCases[x][y].pawnExist) // " Second clic "
            {
                // Mise à jour de l'interface
                Plateau.plateauCases[x][y].pb.Image = Plateau.plateauCases[xSelected][ySelected].pb.Image;
                Plateau.plateauCases[x][y].pawnExist = true;
                Plateau.plateauCases[x][y].pawnTop = Plateau.plateauCases[xSelected][ySelected].pawnTop;

                Plateau.plateauCases[xSelected][ySelected].pb.Image = null;
                Plateau.plateauCases[xSelected][ySelected].pawnExist = false;
                Plateau.plateauCases[xSelected][ySelected].pawnTop = false;

                // TODO : Actualisation de la variable pawnTop à refaire ?

                // Mise à jour des informations Joueurs
                if (playerManager.Joueur1 == Player)
                {
                    playerManager.Joueur1.infos.gameTour = false;
                    playerManager.Joueur2.infos.gameTour = true;
                }else
                {
                    playerManager.Joueur1.infos.gameTour = true;
                    playerManager.Joueur2.infos.gameTour = false;
                }

                pawnSelected = false;
                xSelected = -1;
                ySelected = -1;
            }
        }
    }
}
