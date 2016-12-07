using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ns1;

namespace WindowsFormsApplication2
{
    public partial class loginForm : Form
    {
        ns1.Drag MF = new Drag();
        private string password = string.Empty;
        public loginForm()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            Form form = new mainUI();
            form.Show();
            this.Hide();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = bunifuMetroTextbox1;
        }

        private void loginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void bunifuMetroTextbox1_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void bunifuMetroTextbox2_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void bunifuMetroTextbox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void bunifuMetroTextbox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = e.SuppressKeyPress = true;
                
            }
        }

        private void backgroundPanel_MouseDown(object sender, MouseEventArgs e)
        {

            MF.Grab(this);
        }

        private void backgroundPanel_MouseUp(object sender, MouseEventArgs e)
        {
            MF.Release();
        }

        private void backgroundPanel_MouseMove(object sender, MouseEventArgs e)
        {
            MF.MoveObject();
        }
    }
}
