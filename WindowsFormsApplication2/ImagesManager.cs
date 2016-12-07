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
        static Panel formMain;

        int xSelected = -1;
        int ySelected = -1;

        bool onClick = false;

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
                    RuleAdvanced.isNotCareful(ySelected, xSelected);
                    return;
                }

                if (Player.infos.iscombo)
                {
                    if (!Plateau.plateauCases[ySelected][xSelected].mainCombo)
                    {
                        MessageBox.Show("Vous ne pouvez jouer que le pion 'combo' ");
                        return;
                    }
                }
                
                setCase(xSelected, ySelected);
                onClick = true;
                // Changement de curseur [1]
            }
        }

        private void pawnUp(object sender, MouseEventArgs e) // Relâche
        {
            onClick = false;
            Joueur Player = playerManager.WhosNext();
            try
            {
                Control controlObject = FindControlAtCursor(formMain.FindForm());

                int x = controlObject.Location.X / 50;
                int y = controlObject.Location.Y / 50;

                if (!pawnMoving(x, y))
                {
                    setCase(xSelected, ySelected, getPawnImgByPlayer(Player), true, Player.infos.playerTop);
                }
            }
            catch (Exception ex)
            {
                setCase(xSelected, ySelected, getPawnImgByPlayer(Player), true, Player.infos.playerTop);
                MessageBox.Show("Vous ne pouvez pas vous déplacer sur une case blanche");
            }

            // Changement de curseur [2]
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

        public bool pawnMoving(int x, int y)
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
                int ruleDistance = Rule.distanceOk(y, x, ySelected, xSelected, Player.infos.playerTop);

                if (ruleDistance == 0 && !Plateau.plateauCases[y][x].king)
                {
                    MessageBox.Show("Vous ne pouvez pas faire cela");
                    return false;
                }

                // Mise à jour de l'interface

                Plateau.plateauCases[y][x].pb.Visible = false;
                BunifuAnimatorNS.BunifuTransition transition = new BunifuAnimatorNS.BunifuTransition();
                transition.AnimationType = BunifuAnimatorNS.AnimationType.Transparent;
                transition.Interval = 10;
                transition.ShowSync(Plateau.plateauCases[y][x].pb);

                setCase(x, y, getPawnImgByPlayer(Player), true, Player.infos.playerTop);

                setCase(xSelected, ySelected);

                System.Threading.Thread.Sleep(200);
                Plateau.plateauCases[y][x].pb.Visible = true;

                if (ruleDistance == 2) // Attaque un pion adverse
                {
                    int x_pawn = Rule.x_pawn;
                    int y_pawn = Rule.y_pawn;

                    Plateau.plateauCases[y_pawn][x_pawn].pb.Visible = false;
                    transition = new BunifuAnimatorNS.BunifuTransition();
                    transition.AnimationType = BunifuAnimatorNS.AnimationType.Particles;
                    transition.Interval = 10;
                    transition.ShowSync(Plateau.plateauCases[y][x].pb);

                    setCase(x_pawn, y_pawn);
                    

                    Opponent.infos.pawnAlive--;
                   // MessageBox.Show("Pion(s) restant(s) = " + Opponent.infos.pawnAlive);
                    System.Threading.Thread.Sleep(200);
                    Plateau.plateauCases[y_pawn][x_pawn].pb.Visible = true;

                    if (RuleAdvanced.detectCanEat(Player, x, y))
                    {
                        Player.infos.iscombo = true;
                        Plateau.plateauCases[y][x].mainCombo = true;
                        return true;
                    }else
                    {
                        Player.infos.iscombo = false;
                        Plateau.plateauCases[y][x].mainCombo = false;
                    }
                }

                RuleAdvanced.resetNotCarefulOpponent(Opponent);
                Rule.checkJumpingNotPlayed(Player, x, y);

                // Mise à jour des informations Joueurs
                playerManager.ChangeGameTurn(Player);

                xSelected = -1;
                ySelected = -1;
            }
            return true;
        }

        public Image getPawnImgByPlayer(Joueur Player, bool ishold = false)
        {
            if (Player.infos.playerTop)
            {
                if (ishold)
                {
                    return new Bitmap("pion_1_hold.png");
                }
                return new Bitmap("pion_1.png");
            }
            if (ishold)
            {
                return new Bitmap("pion_2_hold.png");
            }
            return new Bitmap("pion_2.png");
        }

        public static void setCase(int x, int y, Image imgPawn = null, bool isExist = false, bool isTop = false)
        {
            try
            {
                Plateau.plateauCases[y][x].pb.Image = imgPawn;
                Plateau.plateauCases[y][x].pawnExist = isExist;
                Plateau.plateauCases[y][x].pawnTop = isTop;
            }
            catch(Exception ex)
            {
                MessageBox.Show("x = " + x + " | y = " + y);
            }
           
        }

        public static Control FindControlAtPoint(Control container, Point pos)
        {
            Control child;
            foreach (Control c in container.Controls)
            {
                if (c.Visible && c.Bounds.Contains(pos))
                {
                    child = FindControlAtPoint(c, new Point(pos.X - c.Left, pos.Y - c.Top));
                    if (child == null) return c;
                    else return child;
                }
            }
            return null;
        }

        public static Control FindControlAtCursor(Form form)
        {
            Point pos = Cursor.Position;
            if (form.Bounds.Contains(pos))
                return FindControlAtPoint(form, form.PointToClient(pos));
            return null;
        }

        public ImagesManager(Panel _formMain) // constructeur
        {
            formMain = _formMain;
        }
    }
}
