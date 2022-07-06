using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using L4d2_Mod_Manager_Tool.Domain;
using L4d2_Mod_Manager_Tool.Domain.Repository;
using L4d2_Mod_Manager_Tool.Service;
using L4d2_Mod_Manager_Tool.Utility;

namespace L4d2_Mod_Manager_Tool
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 过滤字符串
        /// </summary>
        private string FilterString => textBox_search.Text;

        public Form1()
        {
            InitializeComponent();
            Text = "求生之路2模组管理工具 " + WinformUtility.SoftwareVersion;
            button_clearFilter.Visible = false;
            listView1.ListViewItemSorter = new Widget.ListViewColumnSorter()
            {
                SortColumn = 0,
                Order = SortOrder.Ascending
            };
            UpdateModList();
        }

        /// <summary>
        /// 更新模组文件
        /// </summary>
        private void RefreshModFile()
        {
            // 开始前检查
            var setting = Module.Settings.SettingFP.GetSetting();
            if (!File.Exists(setting.NoVtfExecutablePath))
            {
                WinformUtility.ErrorMessageBox("请先设置no_vtf可执行程序", "环境错误");
                return;
            }
            // 获取模组文件列表
            // 保存数据库（同步）
            // 将文件转换为模组文件（解压vpk），解析Maddoninfo，获取详细信息
            // 入库
            var tasks = VPKServices.ScanAllModFile()
                .Where(mf => !ModFileService.ModFileExists(mf.FilePath))
                .Select(mf => new ExtraModTask(mf)).ToArray();
            // 如果有新增模型，开启任务界面，开始扫描
            if (tasks.Length > 0)
                new Form_RunningTask("扫描模组", tasks).ShowDialog();
            // 最后更新模组列表
            UpdateModList();
        }

        /// <summary>
        /// 更新模组列表
        /// </summary>
        private void UpdateModList()
        {
            listView1.Items.Clear();
            imageList1.Images.Clear();

            int index = 0;

            foreach (var mod in ModOperation.FilterMod(FilterString).Select(ModOperation.GetModDetail))
            {
                var img = SelectImage(mod.Img);
                imageList1.Images.Add(img);

                ListViewItem item = new(new string[] {
                    mod.Name,
                    mod.Vpkid,
                    mod.Author,
                    mod.Tagline
                });
                //item.Text = ModFP.SelectName(mod.img);
                //item.SubItems.Add(mod.vpkId);
                item.ImageIndex = index++;
                item.Tag = mod.Id;
                listView1.Items.Add(item);
            }
        }

        /// <summary>
        /// 为没有创意工坊信息的模组下载创意工坊数据
        /// </summary>
        private void DownloadWorkshopInfoIfDontHave()
        {
            var tasks = ModRepository.Instance.GetMods()
                .Where(ModFP.HaveVpkNumber)
                .Where(Utility.FPExtension.Not<Mod>(ModFP.HaveWorkshopInfo))
                .Select(x => new DownloadWorkshopInfoTask(x));
            new Form_RunningTask("下载创意工坊信息", tasks.ToArray()).ShowDialog();
            UpdateModList();
        }

        private void UpdateModPreview(int modId)
        {
            ModOperation.GetModDetail(modId).Match(detail => {
                widget_ModOverview1.ModPreview = detail.Img;
                widget_ModOverview1.ModName = detail.Name;
                widget_ModOverview1.ModAuthor = detail.Author;
                widget_ModOverview1.ModCategories = detail.Categories;
                widget_ModOverview1.ModDescript = detail.Descript;
            }, () => { });
        }
        #region 定义
        //private class TestMessageTask : TaskFramework.IMessageTask
        //{
        //    public string TaskName { get; private set; }
        //    public TestMessageTask(int i)
        //    {
        //        TaskName = $"正在进行任务{i}...";
        //    }
        //
        //    public void DoTask()
        //    {
        //        System.Threading.Thread.Sleep(10);
        //    }
        //}
        private class ExtraModTask : TaskFramework.IMessageTask
        {
            private ModFile mf;
            public string TaskName { get; private set; }
            public ExtraModTask(ModFile mf)
            {
                TaskName = $"正在解压{mf.FilePath}...";
                this.mf = mf;
            }
            public void DoTask()
            {
                var savedMf = ModFileService.SaveModFile(mf);
                var mod = VPKServices.ExtraMod(savedMf);
                ModRepository.Instance.SaveMod(mod);
            }
        }

        private class DownloadWorkshopInfoTask : TaskFramework.IMessageTask
        {
            private Mod mod;
            public string TaskName { get; private set; }
            public DownloadWorkshopInfoTask(Mod mod)
            {
                TaskName = "下载创意工坊信息，VPKID=" + mod.vpkId + "...";
                this.mod = mod;
            }

            public void DoTask()
            {
                var (newMod, succ) = ModOperation.UpdateWorkshopInfo(mod);
                if (succ)
                {
                    newMod = ModOperation.MoveWorkshopPreviewImage(newMod);
                    ModOperation.UpdateMod(newMod);
                }
            }
        }
        #endregion
        #region UI事件

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listView1.SelectedItems)
            {
                // 取出自定义数据
                int modId = (int)i.Tag;
                var mod = ModRepository.Instance.FindModById(modId);
                mod.Map(x =>
                {
                    ModOperation.DeactiveMod(x);
                    return 0;
                });
            }
        }

        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            button_clearFilter.Visible = !string.IsNullOrEmpty(textBox_search.Text);
            UpdateModList();
        }
        private void button_clearFilter_Click(object sender, EventArgs e)
        {
            textBox_search.Text = "";
        }

        // 模组列表右键菜单
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(listView1, e.Location);
            }
        }

        private void toolStripMenuItem_showInExplorer_Click(object sender, EventArgs e)
        {
            //Module.FileExplorer.FileExplorerUtils.OpenFileExplorerAndSelectItem()
            if (listView1.SelectedItems.Count == 0)
                return;
            int modId = (int)listView1.SelectedItems[0].Tag;
            ModOperation.ShowModInFileExplorer(modId);
        }

        // 刷新只更新列表
        private void toolStripMenuItem_refresh_Click(object sender, EventArgs e)
        {
            UpdateModList();
        }

        private void toolStripMenuItem_scanModFile_Click(object sender, EventArgs e)
        {
            RefreshModFile();
        }

        private void toolStripMenuItem_about_Click(object sender, EventArgs e)
        {
            new Dialog_AboutSoftware().ShowDialog();
        }

        private void tolStripMenuItem_settings_Click(object sender, EventArgs e)
        {
            var form = new Form_Settings();
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog(this);
        }

        private void toolStripMenuItem_downloadWorkshopInfo_Click(object sender, EventArgs e)
        {
            DownloadWorkshopInfoIfDontHave();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            int modId = (int)listView1.SelectedItems[0].Tag;
            ModOperation.OpenModFileInExplorer(modId);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = (sender as ListView).SelectedItems;
            // 只显示第一个选择项
            if(selected.Count > 0)
            {
                UpdateModPreview((int)selected[0].Tag);
            }
        }
        #endregion

        /// <summary>
        /// 选择正确的图片，如果图片不存在或空使用空图片
        /// </summary>
        private static Image SelectImage(string img)
        {
            return LoadImageSafe(img).ValueOr(
                    Image.FromFile(@"Resources/no-image.png"));
        }

        /// <summary>
        /// 安全地载入图片，如果图片不存在或有错返回maybe.none
        /// </summary>
        private static Utility.Maybe<Image> LoadImageSafe(string file)
        {
            try
            {
                return Image.FromFile(file);
            }
            catch
            {
                return Utility.Maybe.None;
            }
        }
    }
}
