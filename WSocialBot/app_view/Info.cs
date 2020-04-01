using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSocialBot.app_view
{
    class Info
    {
        public static void Display(string infoMessage)
        {
            MessageBox.Show(infoMessage, "WSocialBot", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
    }
}
