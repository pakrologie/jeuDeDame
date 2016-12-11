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
        static Panel panelMain;

        int xSelected = -1;
        int ySelected = -1;

        bool onClick = false;

        public ImagesManager(Panel _panelMain) // constructeur
        {
            panelMain = _panelMain;
        }

        private void pawnDown(object sender, MouseEventArgs e) // Clic
        {
            if (!onClick)
            {
                Joueur Player = playerManager.WhosNext();

                PictureBox pawn = (PictureBox)sender;

                xSelected = pawn.Location.X / 50;
                ySelected = pawn.Location.Y / 50;

                if (Player.infos.playerTop != Plateau.plateauCases[ySelected][xSelected].pawnTop)
                {
                    Careful.isNotCareful(ySelected, xSelected);
                    return;
                }

                if (Player.infos.iscombo)
                {
                    if (!Plateau.plateauCases[ySelected][xSelected].mainCombo)
                    {
                        MessageBox.Show("Vous ne pouvez jouer que le pion 'combo'");
                        return;
                    }
                }
                onClick = true;
            }
        }

        private void pawnUp(object sender, MouseEventArgs e) // Relâche
        {
            if (onClick)
            {
                Joueur Player = playerManager.WhosNext();
                try
                {
                    Control controlObject = cursorControl.FindControlAtCursor(panelMain.FindForm());

                    int x = controlObject.Location.X / 50;
                    int y = controlObject.Location.Y / 50;

                    if (!Action.pawnMoving(x, y, xSelected, ySelected))
                    {
                       
                    }
                }
                catch (Exception ex)
                { }
                onClick = false;
            }
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
                        //Plateau.plateauCases[y][x].king = false;
                        Bitmap img = null;

                        if (y < 3) // Pion du haut
                        {
                            img = (Bitmap)getPawnImgByPlayer(true);
                            Plateau.plateauCases[y][x].pawnExist = true;
                            Plateau.plateauCases[y][x].pawnTop = true;
                            Plateau.plateauCases[y][x].king = true;
                        }
                        if (y > 6) // Pion du bas
                        {
                            img = (Bitmap)getPawnImgByPlayer(false);
                            Plateau.plateauCases[y][x].pawnExist = true;
                            Plateau.plateauCases[y][x].pawnTop = false;
                        }

                        pb.Image = img;

                        panelMain.Controls.Add(pb);
                    }
                }
            }
        }
       
        public static void pawnToKing(bool playerTop, int x, int y)
        {
            if (Distance.isLastLine(playerTop, y))
            {
                if (!Plateau.plateauCases[y][x].king)
                {
                    Plateau.plateauCases[y][x].pb.Image = getPawnImgByPlayer(playerTop, true);
                    Plateau.plateauCases[y][x].king = true;
                }
            }
        }

        public static Image getPawnImgByPlayer(bool playerTop, bool isking = false)
        {
            if (playerTop)
            {
                if (isking)
                {
                    return new Bitmap("pion_1_queen.png");
                }
                return new Bitmap("pion_1.png");
            }
            if (isking)
            {
                return new Bitmap("pion_2_queen.png");
            }
            return new Bitmap("pion_2.png");
        }
    }
}
