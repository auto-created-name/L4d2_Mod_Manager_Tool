using L4d2_Mod_Manager_Tool.DTO;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.TaskFramework
{
    public class BackgroundTaskProgressChangedArgs : EventArgs
    {
        public BackgroundTaskProgress Progress { get; private set; }
        public BackgroundTaskProgressChangedArgs(BackgroundTaskProgress progress)
            => Progress = progress;
    }

    public class BackgroundTaskList
    {
        public event EventHandler<BackgroundTaskProgressChangedArgs> OnBackgroundTaskAdded;
        public event EventHandler<BackgroundTaskProgressChangedArgs> OnBackgroundTaskFinished;
        public event EventHandler<BackgroundTaskProgressChangedArgs> OnBackgroundTaskChanged;
        private ConcurrentDictionary<int, BackgroundTask> taskList = new ();
        private int id = 0;
        public BackgroundTask NewTask(string name)
        {
            int newId = Interlocked.Increment (ref id);
            var bt = new BackgroundTask(newId) { Name = name, Progress = 0, Status = string.Empty };
            bt.OnProgressChanged += BackgroundTask_OnProgressChanged;
            bt.OnFinished += BackgroundTask_OnFinished;

            taskList.TryAdd (newId, bt);

            BackgroundTaskProgress btp = DTO(bt);
            OnBackgroundTaskAdded?.Invoke(this, new BackgroundTaskProgressChangedArgs(btp));
            return bt;
        }

        private void BackgroundTask_OnProgressChanged(object sender, EventArgs e)
        {
            var bt = sender as BackgroundTask;
            BackgroundTaskProgress btp = DTO(bt);
            OnBackgroundTaskChanged?.Invoke(this, new BackgroundTaskProgressChangedArgs(btp));
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


            BackgroundTaskProgress btp = DTO(bt);
            OnBackgroundTaskFinished?.Invoke(this, new BackgroundTaskProgressChangedArgs(btp));
        }

        private static BackgroundTaskProgress DTO(BackgroundTask task)
            => new()
            {
                Id = task.Id,
                Name = task.Name,
                Progress = task.Progress,
                Status = task.Status
            };
    }
}
