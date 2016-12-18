using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    class More
    {
        public static string b_str(byte[] buffer, int packetlength = -1)
        {
            if (packetlength != -1)
            {
                return System.Text.Encoding.UTF8.GetString(buffer).Substring(0, packetlength);
            }
            return System.Text.Encoding.UTF8.GetString(buffer);
        }

        public static byte[] s_byte(string packet)
        {
            return System.Text.Encoding.UTF8.GetBytes(packet);
        }

        public static bool isDec(string value)
        {
            int nb;
            return int.TryParse(value, out nb);
        }

        public static int s_int(string str)
        {
            return Convert.ToInt32(str);
        }

        public static string i_str(int value)
        {
            return Convert.ToString(value);
        }
    }
}
