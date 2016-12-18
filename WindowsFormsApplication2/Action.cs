using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication2
{
    class Action
    {
        static Panel panelMain;

        public Action(Panel _panelMain)
        {
            panelMain = _panelMain;
        }
        
        public static void setCase(int x, int y, string imgPawn, bool isExist, bool isTop)
        {
            try
            {
                if (imgPawn != "-1")
                {
                    Plateau.plateauCases[y][x].pb.Image = new Bitmap(imgPawn);
                }
                else
                    Plateau.plateauCases[y][x].pb.Image = null;
                
                Plateau.plateauCases[y][x].pawnExist = isExist;
                Plateau.plateauCases[y][x].pawnTop = isTop;
            }
            catch (Exception ex)
            { }
        }
    }
}
