using L4d2_Mod_Manager_Tool.Domain;
using L4d2_Mod_Manager_Tool.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace L4d2_Mod_Manager_Tool.Widget
{
    public partial class Widget_FilterMod : UserControl
    {
        /// <summary>
        /// 当过滤器被更新时触发
        /// </summary>
        public event EventHandler<ModFilterChangedArgs> OnFilterUpdated;
        private string name = string.Empty;
        private List<string> tags = new();
        private List<string> cats = new();

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
            tags.Add(tagName);
            InvokeOnFilterUpdated();
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
            cats.Add(catName);
            InvokeOnFilterUpdated();
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

        private void InvokeOnFilterUpdated()
        {
            OnFilterUpdated?.Invoke(this, new ModFilterChangedArgs(name, tags, cats));
        }

        #region 回调函数
        private void button_removeTag_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            //删除谓词
            tags.Remove(btn.Text.Substring(3));
            InvokeOnFilterUpdated();
            btn.Dispose();
        }
        private void button_removeCategory_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            //删除谓词
            cats.Remove(btn.Text.Substring(3));
            InvokeOnFilterUpdated();
            btn.Dispose();
        }
        private void button_clearFilter_Click(object sender, EventArgs e)
        {
            textBox_search.Text = "";
        }

        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            button_clearFilter.Visible = !string.IsNullOrEmpty(textBox_search.Text);
            name = (sender as TextBox).Text;
            InvokeOnFilterUpdated();
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

    public class ModFilterChangedArgs : EventArgs
    {
        public string Name { get; private set; }
        public List<string> Tags { get; private set; }
        public List<string> Categories { get; private set; }
        public ModFilterChangedArgs(string name, List<string> tags, List<string> cats)
        {
            Name = name;
            Tags = tags;
            Categories = cats;
        }
    }
}
