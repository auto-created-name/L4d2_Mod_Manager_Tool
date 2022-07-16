using L4d2_Mod_Manager_Tool.Module.FileExplorer;
using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Service
{
    static class ModCrossServer
    {
        /// <summary>
        /// 通过Mod ID获取模组路径（相对路径）
        /// </summary>
        /// <param name="modId"></param>
        /// <returns></returns>
        public static Maybe<string> GetModFileByModId(int modId)
        {
            return ModOperation.FindModById(modId)
                .Bind(mod => ModFileService.FindFileById(mod.FileId))
                .Map(mf => mf.FilePath);
        }



        /// <summary>
        /// 打开资源管理器，选中模组文件
        /// </summary>
        public static void ShowModInFileExplorer(int modId)
        {
            GetModFileByModId(modId)
                .Map(f => FileExplorerUtils.OpenFileExplorerAndSelectItem(
                    L4d2Folder.GetAddonFileFullPath(f)));
        }

        /// <summary>
        /// 使用资源管理器打开模组文件
        /// </summary>
        public static void OpenModFileInExplorer(int modId)
        {
            GetModFileByModId(modId)
                .Map(f => FileExplorerUtils.OpenFileInExplorer(
                    L4d2Folder.GetAddonFileFullPath(f)));
        }
    }
}
