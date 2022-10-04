using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Core.WorkshopInfoModule;
using L4d2_Mod_Manager_Tool.Domain.Repository;
using Infrastructure.Utility;

namespace L4d2_Mod_Manager_Tool.Service.AddonInfoDownload
{
    /// <summary>
    /// 模组信息下载服务
    /// </summary>
    class AddonInfoDownloadService
    {
        private IAddonInfoDownloadStrategy downloadStrategy;

        public string CurrentDownloadStretegyName => downloadStrategy.StrategyName;

        public AddonInfoDownloadService()
        {
            //首先尝试载入steamworks策略，如果失败再载入爬虫策略
            downloadStrategy = SteamworksAddonInfoDownloadStrategy.CreateStrategy()
                .Match<IAddonInfoDownloadStrategy>(
                    FPExtension.Identity,
                    () => new SpiderAddonInfoDownloadStrategy()
            );
        }
        public IEnumerable<WorkshopInfo> DownloadAddonInfos(IEnumerable<VpkId> vpks)
        {
            var newMods = vpks.AsParallel().Select(DownloadAddonInfo).ToArray();
            return newMods.DiscardMaybe();
        }
        /// <summary>
        /// 下载模组信息
        /// </summary>
        /// <param name="m"></param>
        public Maybe<WorkshopInfo> DownloadAddonInfo(VpkId id)
        {
            var info = downloadStrategy.DownloadAddonInfo((ulong)id.Id);
            return info.Map(i =>
                new WorkshopInfo(id)
                {
                    Description = i.Descript,
                    Preview = new ImageFile(i.PreviewImage),
                    Tags = i.Tags.Select(x => new Tag(x)).ToArray(),
                    Title = i.Title
                }
            );
        }
    }
}
