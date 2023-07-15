using Domain.Core;
using Domain.Core.WorkshopInfoModule;
using Domain.ModFile;
using Domain.ModSorter;
using Infrastructure.Utility;
using L4d2_Mod_Manager_Tool.TaskFramework;
using L4d2_Mod_Manager_Tool.Utility;
using L4d2_Mod_Manager_Tool.Widget;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            imageList1.Images.Add(Image.FromFile("Resources/missing.png"));
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
            //// 开始前检查
            //var setting = SettingFP.GetSetting();
            //if (!File.Exists(setting.NoVtfExecutablePath))
            //{
            //    WinformUtility.ErrorMessageBox("请先设置no_vtf可执行程序", "环境错误");
            //    return;
            //}

            //新版行为
            modFileApplication.ScanAndSaveNewModFile();
            await modFileApplication.AnalysisModFileLocalInfoIfDontHaveAsync();
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

        private void UpdateModPreview(int modId)
        {
            if (modId == -1)
            {
                widget_ModOverview1.ShowModOverview = false;
                return;
            }

            var preview = modFileApplication.GetModPreview(modId);
            if (!preview.HasValue)
            {
                widget_ModOverview1.ShowModOverview = false;
            }
            else
            {
                widget_ModOverview1.ModPreview = preview.Value.PreviewImg;
                widget_ModOverview1.ModName = preview.Value.Name;
                widget_ModOverview1.ModAuthor = preview.Value.Author;
                widget_ModOverview1.ModCategories = preview.Value.Categories;
                widget_ModOverview1.ModDescript = preview.Value.Descript;
                widget_ModOverview1.ModTags = preview.Value.Tags;
                widget_ModOverview1.ShowModOverview = true;
            }
        }

        private void WhenModSelected(ListView view, Action<int[]> a)
        {
            var selected = view.SelectedIndices;
            if (selected.Count > 0)
            {
                a(view.SelectedIndices.Cast<int>().ToArray());
            }
        }

        #region UI事件
        #region 模组列表回调
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

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            WhenModSelected(sender as ListView, indices =>
            {
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
                UpdateModPreview(modDetails[selected[0]].Id);
            }
            else
            {
                UpdateModPreview(-1);
            }
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
            if (!detail.ModExisted)
                item.ImageIndex = 2;
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
        #endregion

        #region 模组列表的右键菜单回调
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

        private void toolStripMenuItem_shareMod_Click(object sender, EventArgs e)
        {
            WhenModSelected(listView1, indices =>
            {
                // 生成模组分享码
                var ids = indices.Select(i => modDetails[i].Id).ToArray();
                var shareCode = modFileApplication.GenerateModShareCode(ids);
                if (string.IsNullOrEmpty(shareCode))
                {
                    MessageBox.Show("Ops，没有选择模组或选择的模组都不可分享，分享失败", "共享模组");
                }
                else
                {
                    Clipboard.SetDataObject(shareCode);
                    MessageBox.Show("已将分享码复制到剪贴板，现在可以粘贴给好友分享模组", "共享模组");
                }
            });
        }
        #endregion

        #region 菜单栏的菜单项回调
        // 刷新只更新列表
        private void toolStripMenuItem_refresh_Click(object sender, EventArgs e)
        {
            // modFileApplication.UpdateModBriefList();
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

        private void toolStripMenuItem_subscribeByShareCode_Click(object sender, EventArgs e)
        {
            var clipboardString = Clipboard.GetText();
            if (string.IsNullOrEmpty(clipboardString) 
                || !modFileApplication.IsModShareCodeValid(clipboardString))
            {
                MessageBox.Show("没有检测到模组分享码，请重新复制", "订阅失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                var dialog = new Dialog_SubscribMod(worshopInfoApplication);
                dialog.ShowDialog();
                //var task = modFileApplication.SubscriptModByShareCode(clipboardString);
                //task.ContinueWith(t => MessageBox.Show(t.Result));
            }
        }

        private void ToolStripMenuItem_clearMissingMod_Click(object sender, EventArgs e)
        {
            int clearCount = modFileApplication.ClearMissingModFile();
            UpdateModList();

            // 通知用户
            MessageBox.Show($"共清除{clearCount}条记录", "清除丢失模组记录", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion


        private void widget_FilterMod1_OnFilterUpdated(object sender, ModFilterChangedArgs e)
        {
            modFileApplication.SetModFilter(e.Name, e.Tags, e.Categories);
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