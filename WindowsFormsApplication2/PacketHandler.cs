using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class PacketHandler
    {
        static Panel gamePanel;
        static ListBox gameList;
        static Form MyGameForm;

        public PacketHandler(Panel _gamePanel, ListBox _gameList, Form _MyGameForm)
        {
            gamePanel = _gamePanel;
            gameList = _gameList;
            MyGameForm = _MyGameForm;
        }
    }
}
