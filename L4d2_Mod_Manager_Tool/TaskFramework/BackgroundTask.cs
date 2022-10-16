using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.TaskFramework
{
    /// <summary>
    /// 描述一个正在进行的后台任务
    /// </summary>
    public class BackgroundTask : IDisposable
    {
        public event EventHandler OnProgressChanged;
        public event EventHandler OnFinished;
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public float Progress { get; set; }

        public BackgroundTask(int id)
            => Id = id;

        public void UpdateProgress()
            => OnProgressChanged?.Invoke(this, null);

        public void Dispose()
        {
            OnFinished?.Invoke(this, null);
        }
    }
}
