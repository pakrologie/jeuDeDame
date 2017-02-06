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
    public partial class mainUI : Form
    {   
        private string url = "http://omega-team.net/app/";
        public static string _Username = String.Empty;
        public static string _Password = String.Empty;

        private bool canPlay = true;

        Form MyGameForm;
        
        public mainUI(string Username, string Password)
        {
            this.DoubleBuffered = true;
            InitializeComponent();

            _Username = Username;
            _Password = Password;

            MyGameForm = new gameForm(this);

            MyGameForm.Show();
            MyGameForm.Hide();
           
        }
        
        private void Disconnect()
        {
            Form loginForm = new loginForm();
            _Username = String.Empty;
            _Password = String.Empty;
            canPlay = false;
            loginForm.ShowDialog();
        }

        public void CheckCon(string id, string pw)
        {
            System.Net.WebClient webc = new System.Net.WebClient();
            int response = Convert.ToInt32(webc.DownloadString(url + "check_maintenance.php"));

            if (response == 1)
            {
                MessageBox.Show("Server is going under maintenance, You will be logged off.");
                Disconnect();
                canPlay = false;
                return;
            }
            
            response = Convert.ToInt32(webc.DownloadString(url + "check_user.php?username=" + id + "&password=" + pw));
            switch (response)
            {
                case -1:
                    MessageBox.Show("Unknown error, please call the administrator.");
                    Disconnect();
                    break;
                case 0:
                    MessageBox.Show("Your connection has been altered, please re-log!");
                    Disconnect();
                    canPlay = false;
                    break;
                case 1:
                    canPlay = true;
                    break;

                default:
                    MessageBox.Show("Unknown error, please call the administrator.");
                    break;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void mainUI_Load(object sender, EventArgs e)
        {
            bunifuFormFadeTransition1.ShowAsyc(this);

            System.Threading.Thread th = System.Threading.Thread.CurrentThread;
            th.Name = "MainThread";

            //  System.Threading.Thread securityCheckThread = new System.Threading.Thread(() => CheckCon(_Username, _Password));
            System.Threading.Thread securityCheckThread = new System.Threading.Thread(securityCheck);
            securityCheckThread.Name = "securityCheckThread";
            securityCheckThread.Start();

            label1.Text =  _Username + " !";
         
        }
        private void securityCheck()
        {
            //Check de sécurité toutes les 30s
            int startin = 60 - DateTime.Now.Second;
                var t = new System.Threading.Timer(o => CheckCon(_Username, _Password),
                    null, startin * 1000, 30000);
        }
        
        private void mainUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(1);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        
        private void bunifuTileButton6_Click(object sender, EventArgs e)
        {
            if (canPlay)
            {
                string _Opponent = "a";
                int istop = 0;

                if (_Username == "a")
                {
                    _Opponent = "garillos";
                    istop = 1;
                }
                Client.SendPacket("newMatch " + _Username + " " + istop + " " + _Opponent);
            }
        }
    }
}