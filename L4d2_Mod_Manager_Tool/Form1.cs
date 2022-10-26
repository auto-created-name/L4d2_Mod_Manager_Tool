using Domain.Core;
using Domain.Core.WorkshopInfoModule;
using Domain.ModSorter;
using Domain.Settings;
using Infrastructure.Utility;
using L4d2_Mod_Manager_Tool.Service;
using L4d2_Mod_Manager_Tool.TaskFramework;
using L4d2_Mod_Manager_Tool.Utility;
using L4d2_Mod_Manager_Tool.Widget;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModFileRepository = Domain.ModFile.ModFileRepository;

namespace L4d2_Mod_Manager_Tool
{
    public partial class Form1 : Form
    {
        private List<ModBrief> modDetails = new();
        private int orderHeader = 0;
        private int order = 0;
        private Dictionary<int, string> headers = new();

        private readonly ModFileRepository mfRepo = new();
        private readonly WorkshopInfoRepository workshopInfoRepository = new();
        private readonly App.ModFileApplication modFileApplication;
        private readonly App.WorkshopInfoApplication worshopInfoApplication;
        private readonly BackgroundTaskList backgroundTaskList = new();

        public Form1()
        {
            modFileApplication = new(mfRepo, workshopInfoRepository, backgroundTaskList);
            worshopInfoApplication = new(mfRepo, workshopInfoRepository, backgroundTaskList);

            InitializeComponent();
            SetupControl();
            UpdateModList();

            widget_FilterMod1.OnFilterUpdated += widget_FilterMod1_OnFilterUpdated;
            // 当模组摘要信息列表更新时，重新生成模组信息列表
            modFileApplication.OnModBriefListUpdate += (sender, args) =>
            {
                if (InvokeRequired)
                    Invoke(new Action(() => UpdateModList()));
            };
            //自动更新模组列表
            //RefreshModFile();
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
                $"模组信息下载模式：{worshopInfoApplication.AddonInfoDownloadStretegyName}";

            foreach (ColumnHeader column in listView1.Columns)
                headers.Add(column.Index, column.Text);

            widget_BackgroundTaskList1.BackgroundTaskList = backgroundTaskList;

            UpdateModListColumnHeader(listView1);
        }

        /// <summary>
        /// 更新模组文件
        /// </summary>
        private async Task RefreshModFile()
        {
            // 开始前检查
            var setting = SettingFP.GetSetting();
            if (!File.Exists(setting.NoVtfExecutablePath))
            {
                WinformUtility.ErrorMessageBox("请先设置no_vtf可执行程序", "环境错误");
                return;
            }

            //新版行为
            modFileApplication.ScanAndSaveNewModFile();
            await modFileApplication.AnalysisModFileLocalInfoIfDontHaveAsync();
            //var tasks = VPKServices.ScanAllModFile()
            //    .Where(mf => !ModFileService.ModFileExists(mf.FilePath))
            //    .Select(mf => new ExtraModTask(mf)).ToArray();
            //// 如果有新增模型，开启任务界面，开始扫描
            //if (tasks.Length > 0)
            //    new Form_RunningTask("扫描模组", tasks).ShowDialog();
            //AddonListService.Load();
            // 最后更新模组列表
            //UpdateModList();
        }

        /// <summary>
        /// 更新模组列表
        /// </summary>
        private void UpdateModList()
        {
            listView1.Items.Clear();
            modDetails.Clear();

            var mds = modFileApplication.FilteredModInfo();
            modDetails = mds.ToList();

            listView1.VirtualListSize = modDetails.Count;
            listView1.Invalidate();
        }

        ///// <summary>
        ///// 为没有创意工坊信息的模组下载创意工坊数据
        ///// </summary>
        //private void DownloadWorkshopInfoIfDontHave()
        //{
        //    Progress<float> rep = new(f => {
        //        toolStripProgressBar_backgroundworkProgress.Value = (int)(f * 100);
        //        if (f == 1.0f)
        //        {
        //            AddonListService.Load();
        //            UpdateModList();
        //        }
        //    });

        //    worshopInfoApplication.DownloadWorkshopInfoIfDontHave();

        //    var mods = ModRepository.Instance.GetMods()
        //        .Where(ModFP.HaveVpkNumber)
        //        .Where(FPExtension.Not<Mod>(ModFP.HaveWorkshopInfo));
        //    Service.AddonInfoDownload.AddonInfoDownloadService.BeginDownloadAddonInfos(mods, rep);
        //}

        private void UpdateModPreview(int modId)
        {
            if(modId == -1)
            {
                widget_ModOverview1.ShowModOverview = false;
                return;
            }

            var preview = modFileApplication.GetModPreview(modId);
            if(!preview.HasValue)
            {
                widget_ModOverview1.ShowModOverview = false;
            }
            else
            {
                widget_ModOverview1.ModPreview      = preview.Value.PreviewImg;
                widget_ModOverview1.ModName         = preview.Value.Name;
                widget_ModOverview1.ModAuthor       = preview.Value.Author;
                widget_ModOverview1.ModCategories   = preview.Value.Categories;
                widget_ModOverview1.ModDescript     = preview.Value.Descript;
                widget_ModOverview1.ModTags         = preview.Value.Tags;
                widget_ModOverview1.ShowModOverview = true;
            }
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
        
        //private class DownloadWorkshopInfoTask : TaskFramework.IMessageTask
        //{
        //    private Mod mod;
        //    public string TaskName { get; private set; }
        //    public DownloadWorkshopInfoTask(Mod mod)
        //    {
        //        TaskName = "下载创意工坊信息，VPKID=" + mod.vpkId + "...";
        //        this.mod = mod;
        //    }

        //    public void DoTask()
        //    {
        //        Service.AddonInfoDownload.AddonInfoDownloadService.DownloadAddonInfo(mod);
        //    }
        //}
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
                        bool enable = modFileApplication.GetModStatus(modDetail.Id); 
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
                modFileApplication.ShowModFileInFileExplorer(modId);
            });
        }

        private void toolStripMenuItem_enableMod_Click(object sender, EventArgs e)
        {
            WhenModSelected(listView1, indices =>
                indices.Iter(index =>
                {
                    var detail = modDetails[index];
                    modFileApplication.EnableMod(detail.Id);
                    // 重绘项
                    modDetails[index].Enabled = true;
                    listView1.RedrawItems(index, index, false);
                })
            );
            modFileApplication.SaveModStatus();
        }

        private void toolStripMenuItem_disableMod_Click(object sender, EventArgs e)
        {
            WhenModSelected(listView1, indices =>
                indices.Iter(index =>
                {
                    var detail = modDetails[index];
                    modFileApplication.DisableMod(detail.Id);
                    // 重绘项
                    modDetails[index].Enabled = false;
                    listView1.RedrawItems(index, index, false);
                })
            );
            modFileApplication.SaveModStatus();
        }
        #endregion
        // 刷新只更新列表
        private void toolStripMenuItem_refresh_Click(object sender, EventArgs e)
        {
            UpdateModList();
        }

        private async void toolStripMenuItem_scanModFile_Click(object sender, EventArgs e)
        {
            await RefreshModFile();
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
            _ = worshopInfoApplication.DownloadWorkshopInfoIfDontHaveAsync();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            WhenModSelected(sender as ListView, indices => {
                int modId = modDetails[indices[0]].Id;
                modFileApplication.OpenModFile(modId);
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

        private void widget_FilterMod1_OnFilterUpdated(object sender, ModFilterChangedArgs e)
        {
            modFileApplication.SetModFilter(e.Name, e.Tags, e.Categories);
            UpdateModList();
        }

        private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            var detail = modDetails[e.ItemIndex];
            ListViewItem item = new(new string[] {
                detail.ReadableName,
                detail.FileName,
                detail.ReadableEnabled,
                detail.ReadableAuthor,
                detail.ReadableTagline
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
            modFileApplication.SetModSortMod(headers[orderHeader], order == 0 ? ModSortOrder.Ascending : ModSortOrder.Descending);
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

        private void button_showBackgroundTaskWnd_Click(object sender, EventArgs e)
        {
            widget_BackgroundTaskList1.Visible = true;
        }
        #endregion
    }
}