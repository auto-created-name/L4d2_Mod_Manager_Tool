using Domain.Settings;
using System;
using System.IO;
using System.Windows.Forms;

namespace L4d2_Mod_Manager_Tool
{
    public partial class Form_Settings : Form
    {
        private Setting setting;
        public Form_Settings()
        {
            InitializeComponent();
            setting = SettingFP.GetSetting();
            textBox_gamePath.Text           = setting.GamePath;
            textBox_novtfExecutable.Text    = setting.NoVtfExecutablePath;
        }

        private void SetGamePath(string gp)
        {
            textBox_gamePath.Text = gp;
        }

        private void button_importLocationFromGameFolder_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "left4dead2.exe | left4dead2.exe";
            dialog.Title = "选择left4dead2.exe";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                var folder = Path.GetDirectoryName(dialog.FileName);
                SetGamePath(folder);
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            setting.GamePath            = textBox_gamePath.Text;
            setting.NoVtfExecutablePath = textBox_novtfExecutable.Text;
            SettingFP.SaveSetting(setting);
            Close();
        }

        private void button_novtfSelectExecutable_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "no_vtf(*.exe) | *.exe";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                textBox_novtfExecutable.Text = dialog.FileName;
            }
        }
    }
}
