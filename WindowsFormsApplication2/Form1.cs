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
    public partial class Form1 : Form
    {
        ImagesManager IM;
        Plateau Plt;
        
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            Plt = new Plateau(this);
            IM = new ImagesManager(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Plt.remplirPlateau();
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Plateau.PaintEventForm1(this.Width, this.Height, e);
            IM.createPictureBox();
            playerManager.createPlayers();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Plateau.MouseClickForm1(e, this.CreateGraphics());
        }
    }
}