using L4d2_Mod_Manager_Tool.Domain;
using L4d2_Mod_Manager_Tool.Service;
using L4d2_Mod_Manager_Tool.Domain.ModFilter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using L4d2_Mod_Manager_Tool.Utility;

namespace L4d2_Mod_Manager_Tool.Widget
{
    class MenuItemsZipper
    {
        private ToolStripMenuItem item;
        public MenuItemsZipper()
        {
            item = new ToolStripMenuItem();
        }

        private MenuItemsZipper(ToolStripMenuItem item)
        {
            this.item = item;
        }

        public ToolStripMenuItem CurrentItem => item;

        public bool HaveChildItem(string text)
        {
            return item.DropDownItems.Cast<ToolStripMenuItem>()
                .Where(x => x.Text.Equals(text)).Any();
        }

        public Maybe<MenuItemsZipper> GotoItem(string text)
        {
            var it = 
                item.DropDownItems.Cast<ToolStripMenuItem>()
                .Where(x => x.Text.Equals(text)).FirstElementSafe();

            return it.Select(item => new MenuItemsZipper(item));
        }

        public MenuItemsZipper InsertItem(string text)
        {
            ToolStripMenuItem i = new(text);
            item.DropDownItems.Add(i);
            return this;
        }

        public MenuItemsZipper Top()
        {
            var i = item;
            while (i.OwnerItem != null)
                i = i.OwnerItem as ToolStripMenuItem;
            return new MenuItemsZipper(i);
        }
    }

    public partial class Widget_FilterMod : UserControl
    {
        /// <summary>
        /// 当过滤器被更新时触发
        /// </summary>
        public event EventHandler OnFilterUpdated;

        public Widget_FilterMod()
        {
            InitializeComponent();
        }

        private void OnTagFilterTrigged(string tagName)
        {
            // 添加移除按钮
            Button btn = new();
            btn.Text = $"标签:{tagName}";
            btn.Click += button_removeTag_Click;
            btn.Size = btn.PreferredSize;
            flowLayoutPanel_filter.Controls.Add(btn);

            //增加谓词
            ModOperation.AddModFilterTag(tagName);
            OnFilterUpdated?.Invoke(this, null);
        }

        private void OnCategoryTrigged(string catName)
        {
            // 添加移除按钮
            Button btn = new();
            btn.Text = $"内容:{catName}";
            btn.Click += button_removeCategory_Click;
            btn.Size = btn.PreferredSize;
            flowLayoutPanel_filter.Controls.Add(btn);

            //增加谓词
            ModOperation.AddModFilterCategory(catName);
            OnFilterUpdated?.Invoke(this, null);
        }

        private ToolStripMenuItem[] CreateCategoryMenuItems()
        {
            // 为每一级的菜单构建项
            var pathes = ModCategoryService.Pathes;
            var secs = pathes.Select(path => path.Split('/'));

            MenuItemsZipper zipper = new();
            foreach(var sec in secs)
            {
                zipper = zipper.Top();
                foreach(var word in sec)
                {
                    if (!zipper.HaveChildItem(word))
                        zipper = zipper.InsertItem(word);
                    zipper = zipper.GotoItem(word).ValueOrThrow("菜单项不存在");
                }
                zipper.CurrentItem.Click += (object sender, EventArgs e) => {
                    OnCategoryTrigged((sender as ToolStripMenuItem).Text);
                };
            }

            return zipper.Top().CurrentItem.DropDownItems.Cast<ToolStripMenuItem>().ToArray();
        }

        #region 回调函数
        private void button_removeTag_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            //删除谓词
            ModOperation.RemoveModFilterTag(btn.Text.Substring(3));
            btn.Dispose();

            OnFilterUpdated?.Invoke(this, null);
        }
        private void button_removeCategory_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            //删除谓词
            ModOperation.RemoveModFilterCategory(btn.Text.Substring(3));
            btn.Dispose();

            OnFilterUpdated?.Invoke(this, null);
        }
        private void button_clearFilter_Click(object sender, EventArgs e)
        {
            textBox_search.Text = "";
        }

        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            ModOperation.SetModFilterName((sender as TextBox).Text);
            button_clearFilter.Visible = !string.IsNullOrEmpty(textBox_search.Text);
            OnFilterUpdated?.Invoke(this, null);
        }

        private void button_filter_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ContextMenuStrip cms = new();
            cms.Items.Add(CreateTagMenuItem("Survivors"     , ModTag.SurvivorsTags  , OnTagFilterTrigged));
            cms.Items.Add(CreateTagMenuItem("Infected"      , ModTag.InfectedTags   , OnTagFilterTrigged));
            cms.Items.Add(CreateTagMenuItem("Game Content"  , ModTag.GameContentTags, OnTagFilterTrigged));
            cms.Items.Add(CreateTagMenuItem("Game Modes"    , ModTag.GameModesTags  , OnTagFilterTrigged));
            cms.Items.Add(CreateTagMenuItem("Weapons"       , ModTag.WeaponsTags    , OnTagFilterTrigged));
            cms.Items.Add(CreateTagMenuItem("Items"         , ModTag.ItemsTags      , OnTagFilterTrigged));
            cms.Show(btn, new Point(20, 20));
        }

        private void button_category_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ContextMenuStrip cms = new();
            cms.Items.AddRange(CreateCategoryMenuItems());
            cms.Show(btn, new Point(20, 20));
        }
        #endregion

        private static ToolStripMenuItem CreateTagMenuItem(string name, IEnumerable<string> tags, Action<string> OnTrigged)
        {
            var tmi = new ToolStripMenuItem(name);
            tmi.DropDownItems.AddRange(tags.Select(x => {
                var item = new ToolStripMenuItem(x);
                item.Click += (object sender, EventArgs e) => OnTrigged((sender as ToolStripMenuItem).Text);
                return item;
            }).ToArray());
            return tmi;
        }
    }
}
