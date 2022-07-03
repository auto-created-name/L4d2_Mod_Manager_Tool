using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using L4d2_Mod_Manager.Module.Settings;
using L4d2_Mod_Manager.Utility;
using L4d2_Mod_Manager_Tool.Utility;

namespace L4d2_Mod_Manager
{
    public partial class Form_Settings : Form
    {
        private BindingList<string> locationList;
        private Setting setting;
        public Form_Settings()
        {
            InitializeComponent();
            setting = SettingFP.GetSetting();
            locationList = new();
            setting.modFileFolder.Iter(x => locationList.Add(x));

            listBox_modFileLocation.DataSource  = locationList;
            textBox_novtfExecutable.Text        = setting.NoVtfExecutablePath;
            textBox_vpkExecutable.Text          = setting.VPKExecutablePath;
        }

        /// <summary>
        /// 删除选中的模组文件路径
        /// </summary>
        private void DeleteSelectedModFolder()
        {
            listBox_modFileLocation.SelectedItems
                .Cast<string>().ToArray()
                .Iter(p => locationList.Remove(p));
        }

        private void AddModFolder(string folder)
        {

            if (locationList.Contains(folder))
            {
                WinformUtility.ErrorMessageBox("路径已存在", "添加路径错误");
                return;
            }

            if (!Directory.Exists(folder))
            {

                WinformUtility.ErrorMessageBox($"路径\"{folder}\"不存在", "添加路径错误");
                return;
            }

            locationList.Add(folder);
        }

        private void button_addLocation_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new();
            if(dialog.ShowDialog() == DialogResult.OK)
                AddModFolder(dialog.SelectedPath);
        }

        private void button_RemoveLocation_Click(object sender, EventArgs e)
        {
            DeleteSelectedModFolder();
        }

        private void listBox_modFileLocation_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedModFolder();
            }
        }

        private void button_importLocationFromGameFolder_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "left4dead2.exe | left4dead2.exe";
            dialog.Title = "选择left4dead2.exe";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                var folder = Path.GetDirectoryName(dialog.FileName);
                var addonFolder = Path.Combine(folder, "left4dead2", "addons");
                var workshopFolder = Path.Combine(addonFolder, "workshop");

                AddModFolder(addonFolder);
                AddModFolder(workshopFolder);
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            setting.modFileFolder = locationList.ToList();
            setting.NoVtfExecutablePath = textBox_novtfExecutable.Text;
            setting.VPKExecutablePath = textBox_vpkExecutable.Text;
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

        private void button_vpkSelectExecutable_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "vpk(*.exe) | *.exe";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox_vpkExecutable.Text = dialog.FileName;
            }
        }
    }
}
