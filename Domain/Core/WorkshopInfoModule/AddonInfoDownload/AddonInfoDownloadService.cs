using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Core.WorkshopInfoModule;
using Infrastructure.Utility;
using System.Collections.Immutable;
using Infrastructure;

namespace Domain.Core.WorkshopInfoModule.AddonInfoDownload
{
    /// <summary>
    /// 模组信息下载服务
    /// </summary>
    public class AddonInfoDownloadService
    {
        private IAddonInfoDownloadStrategy downloadStrategy;

        public string CurrentDownloadStretegyName => downloadStrategy.StrategyName;

        public AddonInfoDownloadService()
        {
            //根据SteamWorks初始化状态选择策略
            downloadStrategy = SteamWorks.IsInitSuccess ? new SteamworksAddonInfoDownloadStrategy() : new SpiderAddonInfoDownloadStrategy();
        }
        public async Task<WorkshopInfo[]> DownloadAddonInfosAsync(IEnumerable<VpkId> vpks)
        {
            List<WorkshopInfo> res = new();
            foreach (var vpk in vpks)
            {
                var wi = await DownloadAddonInfo(vpk);
                wi.Match(some => res.Add(some), ()=>{ });
            }
            return res.ToArray();
            //var newMods = vpks.AsParallel().Select(x => DownloadAddonInfo(x)).ToArray();
            //return newMods.DiscardMaybe().ToArray();
        }
        /// <summary>
        /// 下载模组信息
        /// </summary>
        /// <param name="m"></param>
        public async Task<Maybe<WorkshopInfo>> DownloadAddonInfo(VpkId id)
        {
            var info = await downloadStrategy.DownloadAddonInfoAsync((ulong)id.Id).ConfigureAwait(false);
            return info.Bind(x => ConvertIfNotEmpty(id, x));
        }

        private Maybe<WorkshopInfo> ConvertIfNotEmpty(VpkId id, ModWorkshopInfo i) 
            => i.IsEmpty ? Maybe.None 
            : new WorkshopInfo(id)
            {
                Description = i.Descript,
                Preview = new ImageFile(i.PreviewImage),
                Tags = i.Tags.Select(x => new Tag(x)).ToArray(),
                Title = i.Title,
                Autor = i.Author
            };
    }
}
