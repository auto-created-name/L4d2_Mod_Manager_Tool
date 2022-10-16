using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L4d2_Mod_Manager_Tool.Widget
{
    public partial class Widget_BackgroundTaskList : UserControl
    {
        public Widget_BackgroundTaskList()
        {
            InitializeComponent();
        }

        private void button_hide_Click(object sender, EventArgs e)
        {
            Visible = false;
        }
    }
}
