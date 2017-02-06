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
            panelMain.Invoke((MethodInvoker)delegate ()
            {
                panelMain.Enabled = true;
            });
        }

        public static void makeTransition(int type, int x, int y)
        {
            panelMain.Invoke((MethodInvoker)delegate ()
            {
                panelMain.Enabled = false;
            });

            Plateau.plateauCases[y][x].pb.Invoke((MethodInvoker)delegate ()
            {
                Plateau.plateauCases[y][x].pb.Visible = false;
            });

            BunifuAnimatorNS.BunifuTransition transition = new BunifuAnimatorNS.BunifuTransition();
            transition.AnimationCompleted += (transitionAnimationCompleted);
            transition.AnimationType = (BunifuAnimatorNS.AnimationType)type;
            transition.Interval = 10;
            transition.ShowSync(Plateau.plateauCases[y][x].pb);

            Plateau.plateauCases[y][x].pb.Invoke((MethodInvoker)delegate ()
            {
                Plateau.plateauCases[y][x].pb.Visible = true;
            });
        }
    }
}
