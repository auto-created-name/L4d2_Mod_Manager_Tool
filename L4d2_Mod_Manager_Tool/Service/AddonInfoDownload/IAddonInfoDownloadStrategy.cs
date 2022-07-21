using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L4d2_Mod_Manager_Tool.Domain;
using L4d2_Mod_Manager_Tool.Utility;

namespace L4d2_Mod_Manager_Tool.Service.AddonInfoDownload
{
    /// <summary>
    /// 模组信息下载策略
    /// </summary>
    interface IAddonInfoDownloadStrategy
    {
        Maybe<ModWorkshopInfo> DownloadAddonInfo(ulong vpkid);
        string StrategyName { get; }
    }
}
