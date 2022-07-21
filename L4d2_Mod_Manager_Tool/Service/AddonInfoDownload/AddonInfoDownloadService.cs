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
    /// 模组信息下载服务
    /// </summary>
    static class AddonInfoDownloadService
    {
        private static IAddonInfoDownloadStrategy downloadStrategy;

        public static string CurrentDownloadStrtegyName => downloadStrategy.StrategyName;

        public static void Load()
        {
            //首先尝试载入steamworks策略，如果失败再载入爬虫策略
            downloadStrategy = SteamworksAddonInfoDownloadStrategy.CreateStrategy()
                .Match<IAddonInfoDownloadStrategy>(
                    FPExtension.Identity,
                    () => new SpiderAddonInfoDownloadStrategy()
            );
        }

        /// <summary>
        /// 下载模组信息
        /// </summary>
        /// <param name="m"></param>
        public static void DownloadAddonInfo(Mod m)
        {
            var vpkId = ulong.Parse(m.vpkId);
            var info = downloadStrategy.DownloadAddonInfo(vpkId);
            var newMod = info.Map(i =>
                m with
                {
                    WorkshopTitle = i.Title,
                    WorkshopDescript = i.Descript,
                    Tags = i.Tags,
                    WorkshopPreviewImage = i.PreviewImage
                }).ValueOr(m);
            ModOperation.UpdateMod(newMod);
        }
    }
}
