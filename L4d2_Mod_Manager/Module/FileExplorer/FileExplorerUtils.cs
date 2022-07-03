using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace L4d2_Mod_Manager.Module.FileExplorer
{
    public static class FileExplorerUtils
    {
        /// <summary>
        /// 打开资源管理器，选中制定项
        /// </summary>
        /// <param name="file"></param>
        public static void OpenFileExplorerAndSelectItem(string file)
        {
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe");
            psi.Arguments = $"/select,{file}";
            Process.Start(psi);
        }

        /// <summary>
        /// 使用资源管理器打开文件
        /// </summary>
        public static void OpenFileInExplorer(string file)
        {
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe");
            psi.Arguments = file;
            Process.Start(psi);
        }
    }
}
