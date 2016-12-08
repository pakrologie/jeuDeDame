using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.Framework.Lib;

namespace WindowsFormsApplication2
{

    public partial class gameForm : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        ImagesManager IM;
        Plateau Plt;
        Animation An;

        public bool boardCreated = false;

        public gameForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            Plt = new Plateau(gamePanel);
            IM = new ImagesManager(gamePanel);
            An = new Animation(gamePanel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Plt.remplirPlateau();
        }
        
        private void gamePanel_Paint(object sender, PaintEventArgs e)
        {
            if (!boardCreated)
            {
                Plateau.PaintEventForm1(gamePanel.Width, gamePanel.Height, e);
                IM.createPictureBox();
                playerManager.createPlayers();
                boardCreated = true;
            }
        }

        private void gameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void leftPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void leftPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void leftPanel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void leftPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void rightPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void rightPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void rightPanel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          Color.FromArgb(0, 155, 212));
    
            e.DrawBackground();
            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, Brushes.White, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        private void listBox1_MouseLeave(object sender, EventArgs e)
        {
            listBox1.SelectedItem = null;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void listBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          Color.FromArgb(0, 187, 255));

            e.DrawBackground();
            e.Graphics.DrawString(listBox2.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        private void listBox2_MouseLeave(object sender, EventArgs e)
        {
            listBox2.SelectedItem = null;
        }

        private void bunifuFlatButton12_Click(object sender, EventArgs e)
        {
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}