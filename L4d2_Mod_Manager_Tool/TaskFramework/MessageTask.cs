using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.TaskFramework
{
    public interface IMessageTask
    {
        string TaskName { get; }
        void DoTask();
    }
}
