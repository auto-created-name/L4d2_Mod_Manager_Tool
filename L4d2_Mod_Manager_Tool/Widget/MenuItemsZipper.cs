using Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
}
