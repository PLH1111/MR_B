using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monitor.Common
{
    public class MessageBoxHelper
    {
        public static void ShowError(string error)
        {
            ShowMeaasge(error, "Error", MessageBoxIcon.Error);
        }

        public static void ShowInfo(string info)
        {
            ShowMeaasge(info, "Tips", MessageBoxIcon.Information);
        }

        public static void ShowMeaasge(string info, string tips, MessageBoxIcon icon)
        {
            MessageBox.Show(info, tips, MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, false);
        }
    }
}
