using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class gameForm : Form
    {
        ImagesManager IM;
        Plateau Plt;

        public gameForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            Plt = new Plateau(gamePanel);
            IM = new ImagesManager(gamePanel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Plt.remplirPlateau();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
           
        }

        private void gamePanel_Paint(object sender, PaintEventArgs e)
        {
            Plateau.PaintEventForm1(gamePanel.Width, gamePanel.Height, e);
            IM.createPictureBox();
            playerManager.createPlayers();
        }

        private void gamePanel_MouseClick(object sender, MouseEventArgs e)
        {
            Plateau.MouseClickForm1(e, gamePanel.CreateGraphics());
        }
    }
}