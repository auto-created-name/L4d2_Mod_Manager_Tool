using Domain.Core;
using L4d2_Mod_Manager_Tool.App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L4d2_Mod_Manager_Tool
{
    public partial class Dialog_SubscribMod : Form
    {
        private readonly WorkshopInfoApplication _workshopInfoApplication;
        private (long, string)[] _buf;

        /// <summary>
        /// 同时切换所有模组列表项的选中状态
        /// </summary>
        /// <param name="stat">新状态</param>
        private void ToggleEveryModSelectStatus(bool stat)
        {
            Enumerable.Range(0, checkedListBox1.Items.Count).ToList().
                ForEach(i => checkedListBox1.SetItemChecked(i, stat));
        }

        public Dialog_SubscribMod(WorkshopInfoApplication workshopInfoApplication)
        {
            InitializeComponent();
            _workshopInfoApplication = workshopInfoApplication;

            LoadSubscribModInfo();
        }

        private void LoadSubscribModInfo()
        {
            // 在主界面中已经确认过分享码的合法性
            string shareText = Clipboard.GetText();

            // 获取模组信息，显示在列表中
            _ = _workshopInfoApplication.RequestShareCodeInfo(shareText).ContinueWith(t =>
            {
                checkedListBox1.Invoke(new Action(() => 
                {
                    // 将结果(vpkid, 模组名称)数组记录下来
                    _buf = t.Result;
                    checkedListBox1.Items.Clear();
                    checkedListBox1.Items.AddRange(_buf.Select(p => p.Item2).ToArray());
                }));
            }).ConfigureAwait(false);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ToggleEveryModSelectStatus(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 用选中的索引从缓存中获取选中模组的VPKID
            var ids = checkedListBox1.CheckedIndices.Cast<int>()
                .Select(i => _buf[i].Item1)
                .Select(l => new VpkId(l));
            _workshopInfoApplication.SubscribeMods(ids);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ToggleEveryModSelectStatus(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
