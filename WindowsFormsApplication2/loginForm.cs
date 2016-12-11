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
        private ns1.Drag MF = new Drag();
        private string url = "http://omega-team.net/app/";
        public string Username = String.Empty;
        public string Password = String.Empty;

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
        public void CheckCon(string id, string pw)
        {
            if (bunifuMetroTextbox1.Text == String.Empty || bunifuMetroTextbox2.Text == String.Empty)
            {
                MessageBox.Show("Please fill all the blanks!");
            }
            else
            {
                System.Net.WebClient webc = new System.Net.WebClient();
                int response = Convert.ToInt32(webc.DownloadString(url + "check_maintenance.php"));
                switch (response)
                {
                    default:
                        MessageBox.Show("Unknown error, please call the administrator.");
                        break;
                    case 0:
                        goto Next;
                    case 1:
                        MessageBox.Show("Maintenance, please check back later!");
                        return;
                }

                Next:
                response = Convert.ToInt32(webc.DownloadString(url + "check_user.php?username=" + id + "&password=" + pw));
                switch (response)
                {
                    case -1:
                        MessageBox.Show("Unknown error, please call the administrator.");
                        break;
                    case 0:
                        MessageBox.Show("Wrong credentials!");
                        break;
                    case 1:
                        Username = id;
                        Password = pw;

                        Form form = new mainUI(Username, Password);
                        form.Show();
                        //Animation
                        form.Visible = false;
                        BunifuAnimatorNS.BunifuTransition transition = new BunifuAnimatorNS.BunifuTransition();
                        transition.AnimationType = BunifuAnimatorNS.AnimationType.Transparent;
                        transition.Interval = 20;
                        transition.ShowSync(form);
                        form.Visible = true;
                        //Fin animation
                        this.Hide();
                        break;

                    default:
                        MessageBox.Show("Unknown error, please call the administrator.");
                        break;
                }
            }

        }
        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            CheckCon(bunifuMetroTextbox1.Text, bunifuMetroTextbox2.Text);
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = bunifuMetroTextbox1;
        }

        private void loginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void bunifuMetroTextbox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = e.SuppressKeyPress = true;
                CheckCon(bunifuMetroTextbox1.Text, bunifuMetroTextbox2.Text);
            }
        }

        private void bunifuMetroTextbox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = e.SuppressKeyPress = true;
                CheckCon(bunifuMetroTextbox1.Text, bunifuMetroTextbox2.Text);

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
