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
                PictureBox pawn = (PictureBox)sender;

                xSelected = pawn.Location.X / 50;
                ySelected = pawn.Location.Y / 50;

                onClick = true;
            }
        }

        private void pawnUp(object sender, MouseEventArgs e) // Relâche
        {
            if (onClick)
            {
                try
                {
                    Control controlObject = cursorControl.FindControlAtCursor(panelMain.FindForm());

                    int x = controlObject.Location.X / 50;
                    int y = controlObject.Location.Y / 50;

                    Client.SendPacket("select " + x + " " + y + " " + xSelected + " " + ySelected);
                    //pawnMoving
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

                        Bitmap img = null;

                        if (y < 3) // Pion du haut
                        {
                            img = (Bitmap)getPawnImgByPlayer(true);
                  
                        }
                        if (y > 6) // Pion du bas
                        {
                            img = (Bitmap)getPawnImgByPlayer(false);
                        }

                        pb.Image = img;

                        try
                        {
                            panelMain.Controls.Add(pb);
                        }catch(Exception ex)
                        { }
                    }
                }
            }
        }

        public void resetPlateau()
        {
            for (int y = 0; y < Plateau.countHorizontal; y++)
            {
                for (int x = 0; x < Plateau.countVertical; x++)
                {
                    if (Plateau.plateauCases[y][x].isBlack)
                    {
                        Plateau.plateauCases[y][x].pb.Image = null;
                        if (y < 3) // Pion du haut
                        {
                            Plateau.plateauCases[y][x].pb.Image = (Bitmap)getPawnImgByPlayer(true);

                        }
                        if (y > 6) // Pion du bas
                        {
                            Plateau.plateauCases[y][x].pb.Image = (Bitmap)getPawnImgByPlayer(false);
                        }
                    }
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