using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class Animation
    {
        static Panel panelMain;

        public Animation(Panel _panelMain)
        {
            panelMain = _panelMain;
        }

        private static void transitionAnimationCompleted(object sender, BunifuAnimatorNS.AnimationCompletedEventArg e)
        {
            if (panelMain.InvokeRequired)
            {
                panelMain.Invoke(new changeEnabledPanel(changeEnabledPanel_));
            }
        }
        
        delegate void changeEnabledPanel();
        public static void changeEnabledPanel_()
        {
            panelMain.Enabled = true;
        }

        public static void makeTransition(int type, int x, int y)
        {
            panelMain.Enabled = false;
            Plateau.plateauCases[y][x].pb.Visible = false;

            BunifuAnimatorNS.BunifuTransition transition = new BunifuAnimatorNS.BunifuTransition();
            transition.AnimationCompleted += (transitionAnimationCompleted);
            transition.AnimationType = (BunifuAnimatorNS.AnimationType)type;
            transition.Interval = 10;
            transition.ShowSync(Plateau.plateauCases[y][x].pb);
        }
    }
}
