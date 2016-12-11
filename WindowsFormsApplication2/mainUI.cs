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
        private string _Username = String.Empty;
        private string _Password = String.Empty;
        private bool canPlay = true;

        public mainUI(string Username, string Password)
        {
            InitializeComponent();
            _Username = Username;
            _Password = Password;
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
            switch (response)
            {
                default:
                    break;
                case 0:
                    goto Next;
                case 1:
                    MessageBox.Show("Server is going under maintenance, You will be logged off.");
                    Disconnect();
                    canPlay = false;
                    return;
            }

            Next:
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
            if (canPlay)
            {
                Form form = new gameForm();
                form.Show();
                this.Hide();
            }
            else
            {

            }
        }

        private void mainUI_Load(object sender, EventArgs e)
        {
            System.Threading.Thread th = System.Threading.Thread.CurrentThread;
            th.Name = "MainThread";

            //  System.Threading.Thread securityCheckThread = new System.Threading.Thread(() => CheckCon(_Username, _Password));
            System.Threading.Thread securityCheckThread = new System.Threading.Thread(securityCheck);
            securityCheckThread.Name = "securityCheckThread";
            securityCheckThread.Start();

            label1.Text = "Salut " + _Username + ",";
            label2.Text = "Ton mdp est " + _Password;
         
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
            Environment.Exit(0);
        }
    }
}
