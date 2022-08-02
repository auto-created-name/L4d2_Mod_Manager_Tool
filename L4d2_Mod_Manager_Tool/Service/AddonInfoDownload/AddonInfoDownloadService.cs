using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L4d2_Mod_Manager_Tool.Domain;
using L4d2_Mod_Manager_Tool.Domain.Repository;
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

        public static void BeginDownloadAddonInfos(IEnumerable<Mod> mods, IProgress<float> rep)
        {
            DownloadAddonInfos(mods, rep);
        }


        private static async Task DownloadAddonInfos(IEnumerable<Mod> mods, IProgress<float> rep)
        {
            await Task.Run(() =>
            {
                rep.Report(0);
                int finished = 0;
                int count = mods.Count();
                var newMods = mods.AsParallel().Select(x => {
                    var newMod = DownloadAddonInfo(x);
                    rep.Report(++finished / (float)count);
                    return newMod;
                }).ToArray();
                rep.Report(1);
                ModRepository.Instance.UpdateRange(newMods);
            });
        }
        /// <summary>
        /// 下载模组信息
        /// </summary>
        /// <param name="m"></param>
        public static Mod DownloadAddonInfo(Mod m)
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
            return newMod;
            //ModOperation.UpdateMod(newMod);
        }
    }
}
