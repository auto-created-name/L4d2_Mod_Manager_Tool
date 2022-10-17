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
    public partial class Widget_BackgroundTask : UserControl
    {
        public event EventHandler OnTaskCancelRequested;
        private int _id = 0;
        public Widget_BackgroundTask()
        {
            InitializeComponent();
        }

        public int Id { get;set; }
        public string TaskName
        {
            get => label_taskName.Text;
            set => label_taskName.Text = value;
        }

        public string TaskStatus
        {
            get => label_status.Text;
            set => label_status.Text = value;
        }

        public int Progress
        {
            get => progressBar_taskProgress.Value;
            set => progressBar_taskProgress.Value = value;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            OnTaskCancelRequested?.Invoke(this, null);
        }
    }
}
