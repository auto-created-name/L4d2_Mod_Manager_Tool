using System.Windows.Forms;

namespace L4d2_Mod_Manager_Tool.Utility
{
    public static class WinformUtility
    {

        public static void ErrorMessageBox(string content, string caption)
        {
            MessageBox.Show(content, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
