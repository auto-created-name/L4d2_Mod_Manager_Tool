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
        private List<ModDetail> modDetails = new();
        private int orderHeader = 0;
        private int order = 0;
        private Dictionary<int, string> headers = new();

        public Form1()
        {
            InitializeComponent();
            SetupControl();
            UpdateModList();

            widget_FilterMod1.OnFilterUpdated += widget_FilterMod1_OnFilterUpdated;
        }

        private void SetupControl()
        {
            Text = "求生之路2模组管理工具 " + WinformUtility.SoftwareVersion;
            listView1.VirtualMode = true;

            imageList1.Images.Add(Image.FromFile("Resources/off.png"));
            imageList1.Images.Add(Image.FromFile("Resources/on.png"));
            imageList1.Images.Add("ascending", Image.FromFile("Resources/ascending.png"));
            imageList1.Images.Add("descending", Image.FromFile("Resources/descending.png"));

            toolStripStatusLabel_addonInfoDownloadStrategy.Text = 
                "模组信息下载模式：" + 
                Service.AddonInfoDownload.AddonInfoDownloadService.CurrentDownloadStrtegyName;

            foreach (ColumnHeader column in listView1.Columns)
                headers.Add(column.Index, column.Text);
            UpdateModListColumnHeader(listView1);
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
            AddonListService.Load();
            // 最后更新模组列表
            UpdateModList();
        }

        /// <summary>
        /// 更新模组列表
        /// </summary>
        private void UpdateModList()
        {
            listView1.Items.Clear();
            modDetails.Clear();

            modDetails = ModOperation.FilteredModInfo().ToList();
            //modDetails.Sort(new ModDetailNameComparer());
            listView1.VirtualListSize = modDetails.Count;
            listView1.Invalidate();

            //for(int i = 0; i < modDetails.Count; ++i)
            //{
            //    imageList1.Images.Add(SelectImage(modDetails[i].Img));
            //}
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
            ModOperation.GetModPreviewInfo(modId).Match(previewInfo =>
            {
                widget_ModOverview1.ModPreview = previewInfo.PreviewImg;
                widget_ModOverview1.ModName = previewInfo.Name;
                widget_ModOverview1.ModAuthor = previewInfo.Author;
                widget_ModOverview1.ModCategories = previewInfo.Categories;
                widget_ModOverview1.ModDescript = previewInfo.Descript;
                widget_ModOverview1.ModTags = previewInfo.Tags;
                widget_ModOverview1.ShowModOverview = true;
            }, () =>
            {
                widget_ModOverview1.ShowModOverview = false;
            });
        }

        private void WhenModSelected(ListView view, Action<int[]> a)
        {
            var selected = view .SelectedIndices;
            if (selected.Count > 0)
            {
                a(view.SelectedIndices.Cast<int>().ToArray());
            }
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
                Service.AddonInfoDownload.AddonInfoDownloadService.DownloadAddonInfo(mod);
            }
        }
        #endregion
        #region UI事件

        // 模组列表右键菜单
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                WhenModSelected((sender as ListView), indices =>
                {
                    if (indices.Length == 1)
                    {
                        var modDetail = modDetails[indices[0]];
                        bool enable = AddonListService.IsModEnabled(modDetail.Id);
                        // 启用模组、禁用模组按钮的正确设置
                        contextMenuStrip1.Items.Find("toolStripMenuItem_enableMod", false)[0].Enabled = !enable;
                        contextMenuStrip1.Items.Find("toolStripMenuItem_disableMod", false)[0].Enabled = enable;
                    }
                    else
                    {
                        // 多选时无视开启/关闭
                        contextMenuStrip1.Items.Find("toolStripMenuItem_enableMod", false)[0].Enabled = true;
                        contextMenuStrip1.Items.Find("toolStripMenuItem_disableMod", false)[0].Enabled = true;
                    }
                    contextMenuStrip1.Show(listView1, e.Location);
                });
            }
        }
        #region 模组列表菜单回调
        private void toolStripMenuItem_showInExplorer_Click(object sender, EventArgs e)
        {
            WhenModSelected(listView1, indices => 
            {
                int modId = modDetails[indices[0]].Id;
                ModCrossServer.ShowModInFileExplorer(modId);
            });
        }

        private void toolStripMenuItem_enableMod_Click(object sender, EventArgs e)
        {
            WhenModSelected(listView1, indices =>
                indices.Iter(index =>
                {
                    var detail = modDetails[index];
                    AddonListService.SetModEnabled(detail.Id, true);
                    // 重绘项
                    modDetails[index] = detail with { Enabled = true };
                    listView1.RedrawItems(index, index, false);
                })
            );
            AddonListService.ApplyAddonList();
        }

        private void toolStripMenuItem_disableMod_Click(object sender, EventArgs e)
        {
            WhenModSelected(listView1, indices =>
                indices.Iter(index =>
                {
                    var detail = modDetails[index];
                    AddonListService.SetModEnabled(detail.Id, false);
                    // 重绘项
                    modDetails[index] = detail with { Enabled = false };
                    listView1.RedrawItems(index, index, false);
                })
            );
            AddonListService.ApplyAddonList();
        }
        #endregion
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
            WhenModSelected(sender as ListView, indices => {
                int modId = modDetails[indices[0]].Id;
                ModCrossServer.OpenModFileInExplorer(modId);
            });
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = (sender as ListView).SelectedIndices;
            // 只显示第一个选择项
            if (selected.Count > 0)
            {
                UpdateModPreview(modDetails[selected[0]].Id) ;
            }
            else
            {
                UpdateModPreview(-1);
            }
        }

        private void widget_FilterMod1_OnFilterUpdated(object sender, EventArgs e)
        {
            UpdateModList();
        }

        private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            var detail = modDetails[e.ItemIndex];
            ListViewItem item = new(new string[] {
                detail.Name,
                detail.FileName,
                detail.Enabled ? "启用" : "禁用",
                detail.Author,
                detail.Tagline
            });

            item.ImageIndex = detail.Enabled ? 1 : 0;
            e.Item = item;
            item.BackColor = e.ItemIndex % 2 == 0 ? Color.White : SystemColors.Control;
            item.ForeColor = detail.Enabled ? SystemColors.WindowText : SystemColors.GrayText;
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var listview = (sender as ListView);
            var clickedColumn = listview.Columns[e.Column];
            // 重复点击，切换正倒序
            if (orderHeader == clickedColumn.Index)
            {
                order = order switch
                {
                    0 => 1,
                    1 => 0,
                    _ => 0
                };
            }
            else
            {
                orderHeader = clickedColumn.Index;
                order = 0;
            }
            UpdateModListColumnHeader(listview);
            ModOperation.SetModSortMod(headers[orderHeader], order == 0 ? Domain.ModSorter.ModSortOrder.Ascending : Domain.ModSorter.ModSortOrder.Descending);
            UpdateModList();
        }

        private void UpdateModListColumnHeader(ListView listview)
        {
            foreach (ColumnHeader column in listview.Columns)
            {
                if (orderHeader == column.Index)
                {
                    column.Text = headers[column.Index] + (order == 0 ? " ▼" : " ▲");
                }
                else
                {
                    column.Text = headers[column.Index];
                }
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
        private static Maybe<Image> LoadImageSafe(string file)
        {
            try
            {
                return Image.FromFile(file);
            }
            catch
            {
                return Maybe.None;
            }
        }
    }
}