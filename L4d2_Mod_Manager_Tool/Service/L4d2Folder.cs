﻿using L4d2_Mod_Manager_Tool.Module.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Service
{
    /// <summary>
    /// 求生之路2常用文件夹
    /// </summary>
    static class L4d2Folder
    {
        private const string CoreFolderName = "left4dead2";
        private const string AddonFolderName = "addons";
        private const string WorkshopAddonFolderName = "workshop";
        /// <summary>
        /// 游戏核心文件夹
        /// </summary>
        public static string CoreFolder =>
            Path.Combine(SettingFP.GetSetting().GamePath, CoreFolderName);
        /// <summary>
        /// 离线模组文件夹
        /// </summary>
        public static string AddonsFolder =>
            Path.Combine(CoreFolder, AddonFolderName);
        /// <summary>
        /// 创意工坊模组文件夹
        /// </summary>
        public static string WorkshopAddonsFolder =>
            Path.Combine(AddonsFolder, WorkshopAddonFolderName);
    }
}
