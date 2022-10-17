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

        public void BindModel(VOBackgroundTaskModel model)
        {
            _id = model.Id;
            label_taskName.DataBindings.Add("Text", model, "Name");
            progressBar_taskProgress.DataBindings.Add("Value", model, "Progress");
            label_status.DataBindings.Add("Text", model, "Status");
        }
    }
}
