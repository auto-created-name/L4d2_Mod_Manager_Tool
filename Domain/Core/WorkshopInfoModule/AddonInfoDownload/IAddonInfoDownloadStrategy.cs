using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Utility;

namespace Domain.Core.WorkshopInfoModule.AddonInfoDownload
{
    /// <summary>
    /// 模组信息下载策略
    /// </summary>
    interface IAddonInfoDownloadStrategy
    {
        Task<Maybe<ModWorkshopInfo>> DownloadAddonInfoAsync(ulong vpkid);
        string StrategyName { get; }
    }
}
