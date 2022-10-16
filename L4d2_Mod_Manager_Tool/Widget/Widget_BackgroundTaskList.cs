using L4d2_Mod_Manager_Tool.TaskFramework;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace L4d2_Mod_Manager_Tool.Widget
{
    public partial class Widget_BackgroundTaskList : UserControl
    {
        private BackgroundTaskList backgroundTaskList;

        public BackgroundTaskList BackgroundTaskList
        {
            set
            {
                OnBackgroundTaskListChanged(backgroundTaskList, value);
                backgroundTaskList = value;
            }
        }
        private Dictionary<int, Widget_BackgroundTask> taskWidgets = new();

        public Widget_BackgroundTaskList()
        {
            InitializeComponent();
        }

        private void OnBackgroundTaskListChanged(BackgroundTaskList oldVal, BackgroundTaskList newVal)
        {
            if(oldVal != null)
            {
                oldVal.OnBackgroundTaskAdded    -= BackgroundTaskList_OnBackgroundTaskAdded;
                oldVal.OnBackgroundTaskChanged  -= BackgroundTaskList_OnBackgroundTaskChanged;
                oldVal.OnBackgroundTaskFinished -= BackgroundTaskList_OnBackgroundTaskChanged;
            }
            if(newVal != null)
            {
                newVal.OnBackgroundTaskAdded    += BackgroundTaskList_OnBackgroundTaskAdded;
                newVal.OnBackgroundTaskChanged  += BackgroundTaskList_OnBackgroundTaskChanged;
                newVal.OnBackgroundTaskFinished += BackgroundTaskList_OnBackgroundTaskFinished;
            }
        }

        private void BackgroundTaskList_OnBackgroundTaskAdded(object sender, BackgroundTaskProgressChangedArgs args)
        {
            Widget_BackgroundTask widget = new();
            taskWidgets.Add(args.Progress.Id, widget);
            if (flowLayoutPanel1.InvokeRequired) 
                flowLayoutPanel1.Invoke(new Action(() => flowLayoutPanel1.Controls.Add(widget)));
        }

        private void BackgroundTaskList_OnBackgroundTaskChanged(object sender, BackgroundTaskProgressChangedArgs args)
        {

        }

        private void BackgroundTaskList_OnBackgroundTaskFinished(object sender, BackgroundTaskProgressChangedArgs args)
        {
            var widget = taskWidgets[args.Progress.Id];

            if (flowLayoutPanel1.InvokeRequired)
                flowLayoutPanel1.Invoke(new Action(() => {
                    flowLayoutPanel1.Controls.Remove(widget);
                    widget.Dispose();
                }));
        }

        private void button_hide_Click(object sender, EventArgs e)
        {
            Visible = false;
        }
    }
}
