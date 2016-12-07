using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class Plateau
    {
        public const int casesCount = 10;
        public static int recWidth = 50;
        public static int recHeight = 50;

        public static int countHorizontal = 0;
        public static int countVertical = 0;

        public static cases[][] plateauCases = new cases[casesCount][];
        Panel mainForm;

        public Plateau(Panel _mainForm)
        {
            mainForm = _mainForm;
        }

        public struct cases
        {
            public Rectangle Rec;
            public bool isBlack;
            public PictureBox pb;
            public bool pawnExist;
            public bool pawnTop;
            public bool king;
            public bool isnotcareful;
        }

        public void remplirPlateau()
        {
            bool value = false;
            int index = 0;
            int index_plateau = 0;
            int[] ligne = new int[casesCount];

            for (int i = 0; i < (casesCount * casesCount); i++)
            {
                int loca = i + 1;

                if (loca % casesCount == 0 && i != 0)
                {
                    ligne[index] = Convert.ToInt32(value);
                    cases[] nouvelleCases = new cases[casesCount];

                    for (int e = 0; e < casesCount; e++)
                    {
                        nouvelleCases[e].isBlack = Convert.ToBoolean(ligne[e]);
                    }

                    plateauCases[index_plateau] = nouvelleCases;
                    ligne = new int[casesCount];

                    index = 0;
                    index_plateau++;
                }
                else if (i % 2 == 0)
                {
                    ligne[index] = Convert.ToInt32(value);
                    index++;
                    value = !value;
                }
                else
                {
                    ligne[index] = Convert.ToInt32(value);
                    index++;
                    value = !value;
                }
            }
        }

        public static void PaintEventForm1(int width, int height, PaintEventArgs e)
        {

            int countRec = 0;
            int formWidth = width;
            int formHeight = height;
            int penWidth = 1;

            Pen myPen = new Pen(new SolidBrush(Color.Black), penWidth);

            for (int i = recHeight; i <= formHeight; i += recHeight) //dessine les lignes horizontales
            {
                e.Graphics.DrawLine(myPen, 0, i, formWidth, i);
                countHorizontal++;
            }

            for (int i = recWidth; i <= formWidth; i += recWidth) //dessine les lignes verticales
            {
                e.Graphics.DrawLine(myPen, i, 0, i, formHeight);
                countVertical++;
            }

            countRec = countHorizontal * countVertical;

            for (int x = 0; x < countHorizontal; x++) // Récupère les rectangles
            {
                for (int y = 0; y < countVertical; y++)
                {
                    Rectangle newRec = new Rectangle(recHeight * y, recWidth * x, recWidth, recHeight);
                    plateauCases[x][y].Rec = newRec;
                }
            }

            for (int x = 0; x < plateauCases.Length; x++) // Colorie les cases
            {
                for (int y = 0; y < plateauCases[x].Length; y++)
                {
                    if (plateauCases[x][y].isBlack)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.Black), plateauCases[x][y].Rec);
                    }
                }
            }
        }
    }
}
