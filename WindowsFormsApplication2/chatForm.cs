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
    public partial class chatForm : Form
    {
        public chatForm()
        {
            InitializeComponent();
        }

        private void label1_SizeChanged(object sender, EventArgs e)
        {
            label1.Left = (this.ClientSize.Width - label1.Size.Width) / 2;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
