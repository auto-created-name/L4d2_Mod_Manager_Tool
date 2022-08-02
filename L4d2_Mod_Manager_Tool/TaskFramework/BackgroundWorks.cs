using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.TaskFramework
{
    public enum WorkStatus { Started, Changed, Finished }
    /// <summary>
    /// 后台任务状态改变事件参数
    /// </summary>
    public class BackgroundWorkStatusChangedArgs : EventArgs
    {
        /// <summary>
        /// 工作状态
        /// </summary>
        public WorkStatus WorkStatus { get; private set; }
        /// <summary>
        /// 工作描述
        /// </summary>
        public string WorkDescript { get; private set; }
        /// <summary>
        /// 工作进度,0~100
        /// </summary>
        public int WorkProgress { get; private set; }

        public BackgroundWorkStatusChangedArgs(WorkStatus status, string des, int progress)
        {
            WorkStatus = status;
            WorkDescript = des;
            WorkProgress = progress;
        }
    }
    public class BackgroundWorks : IDisposable
    {
        public event EventHandler<BackgroundWorkStatusChangedArgs> OnBackgroundWorkStatusChanged;
        private record Work (string Name, Task[] tasks);
        private ConcurrentQueue<Work> works = new();
        private Thread thread;
        private bool running;
        public static BackgroundWorks Instance { get; } = new BackgroundWorks();

        public void Dispose()
        {
            running = false;
            thread.Join();
        }

        private BackgroundWorks() 
        {
            running = true;
            thread = new(MaintenanceWorks);
            thread.Start();
        }

        ~BackgroundWorks()
        {
            if (running)
                Dispose();
        }

        public void AppendTasks(string name, Task[] tasks)
        {
            works.Enqueue(new Work(name, tasks));
        }

        private void MaintenanceWorks()
        {
            while (running)
            {
                if(works.TryDequeue(out Work work))
                { 
                    OnBackgroundWorkStatusChanged?.Invoke(this, new(WorkStatus.Started, work.Name, 0));
                    int total = work.tasks.Length;
                    int finished = 0;
                    var tasks = work.tasks.Select(x => x.ContinueWith(
                        _ => OnBackgroundWorkStatusChanged?.Invoke(this, new(WorkStatus.Finished, work.Name, ToPercentInt(++finished / (float)total))))).ToArray();
                    Task.WaitAll(tasks, 50000);
                    OnBackgroundWorkStatusChanged?.Invoke(this, new(WorkStatus.Finished, work.Name, 100));
                }
                else
                {
                    Thread.Sleep(2000);
                }
            }
            System.Windows.Forms.MessageBox.Show("Stop MaintenanceWorks");
        }

        private static int ToPercentInt(float f) 
            => (int)(f * 100);
    }
}
