using L4d2_Mod_Manager_Tool.DTO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.TaskFramework
{
    internal class BackgroundTaskProgressChangedArgs : EventArgs
    {
        public BackgroundTaskProgress Progress { get; private set; }
    }

    internal class BackgroundTaskList
    {
        private ConcurrentDictionary<int, BackgroundTask> taskList = new ();
        private int id = 0;
        public BackgroundTask NewTask(string name)
        {
            var bt = new BackgroundTask(++id) { Name = name, Progress = 0, Status = string.Empty };
            bt.OnProgressChanged += BackgroundTask_OnProgressChanged;
            bt.OnFinished += BackgroundTask_OnFinished;

            taskList.TryAdd (id, bt);
            return bt;
        }

        private void BackgroundTask_OnProgressChanged(object sender, EventArgs e)
        {
            var bt = sender as BackgroundTask;
            //Utility.WinformUtility.ErrorMessageBox(bt.Status + bt.Progress.ToString(), bt.Name);
        }

        private void BackgroundTask_OnFinished(object sender, EventArgs e)
        {
            // 结束的后台断开所有消息
            var bt = sender as BackgroundTask;
            bt.OnProgressChanged -= BackgroundTask_OnProgressChanged;
            bt.OnFinished -= BackgroundTask_OnFinished;
            // 从列表中移除
            taskList.TryRemove(bt.Id, out _);
        }
    }
}
