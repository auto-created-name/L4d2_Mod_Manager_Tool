using Domain.Core;
using Domain.Core.SteamWorksModModule;
using Domain.Core.WorkshopInfoModule;
using Domain.Core.WorkshopInfoModule.AddonInfoDownload;
using Domain.ModFile;
using L4d2_Mod_Manager_Tool.TaskFramework;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace L4d2_Mod_Manager_Tool.App
{
    public class WorkshopInfoApplication
    {
        private ModFileRepository modFileRepository;
        private WorkshopInfoRepository workshopInfoRepository;
        private AddonInfoDownloadService addonInfoDownloadService = new();
        private BackgroundTaskList backgroundTaskList;

        public WorkshopInfoApplication(ModFileRepository mfRepo, WorkshopInfoRepository wiRepo, BackgroundTaskList backgroundTaskList)
        {
            modFileRepository = mfRepo;
            workshopInfoRepository = wiRepo;
            this.backgroundTaskList = backgroundTaskList;
        }

        /// <summary>
        /// 从创意工坊获取分享码的信息
        /// </summary>
        /// <param name="shareCode">分享码</param>
        /// <returns>VPKID-名称 对</returns>
        public async Task<(long, string)[]> RequestShareCodeInfo(string shareCode)
        {
            // 只是分析分享码不需要 ModBriefList
            var ids = new ShareCodeServer(null).ParseShareCode(shareCode);
            var publishedFileIds = ids.Select(i => new Steamworks.Data.PublishedFileId { Value = (ulong)i.Id }).ToArray();
            var items = await Steamworks.Ugc.Query.All.WithFileId(publishedFileIds).GetPageAsync(1);
            return items.Value.Entries.Select(item => ((long)item.Id.Value, item.Title)).ToArray();
            //var swMods = ids.Select(id => new SteamWorksMod(id));
            //
            //List<(long, string)> result = new();
            //foreach(var mod in swMods)
            //{
            //    var name = await mod.RequestModName();
            //    result.Add((mod.Id.Id, name));
            //}
            //
            //return result;
        }

        public async Task<string> DownloadWorkshopInfo(long id)
        {
            //var info = await addonInfoDownloadService.DownloadAddonInfo(new VpkId(id));
            SteamWorksMod mod = new(id);
            return await mod.RequestModName();
        }

        public Task DownloadWorkshopInfoIfDontHaveAsync()
        {
            return Task.Run(() =>
            {
                var allVpkIds = modFileRepository.GetAll().Where(x => x.VpkId != VpkId.Undefined).Select(x => x.VpkId);
                var savedVpkIds = workshopInfoRepository.GetAll().Select(x => x.Id);
                // 找到未记录的
                var dontHave = allVpkIds.Except(savedVpkIds).ToArray();

                var cts = new CancellationTokenSource();
                using var btask = backgroundTaskList.NewTask("下载创意工坊信息");
                btask.OnCanceling += (sender, args) => cts.Cancel();
                btask.Status = "正在初始化...";
                btask.Progress = 0;
                btask.UpdateProgress();


                ConcurrentBag<WorkshopInfo> workshopInfoList = new();
                int finishedCount = 0, totalCount = dontHave.Length;
                foreach (var id in dontHave)
                {
                    // 终止
                    if (cts.Token.IsCancellationRequested)
                        break;

                    btask.Status = $"正在下载 {id.Id} ({finishedCount + 1} / {totalCount})";
                    btask.Progress = (int)(finishedCount * 100f / totalCount);
                    btask.UpdateProgress();

                    var wi = addonInfoDownloadService.DownloadAddonInfo(id).ConfigureAwait(false).GetAwaiter().GetResult();
                    workshopInfoList.Add(wi.ValueOr(null));
                    Interlocked.Increment(ref finishedCount);
                }

                //var res = await addonInfoDownloadService.DownloadAddonInfosAsync(dontHave);

                workshopInfoRepository.SaveRange(workshopInfoList.Where(x => x != null));
            });
        }

        /// <summary>
        /// 订阅指定模组
        /// </summary>
        /// <param name="ids">模组列表</param>
        public void SubscribeMods(IEnumerable<VpkId> ids)
        {
            ids.Select(id => new SteamWorksMod(id)).ToList()
                .ForEach(mod => mod.Subscribe());
        }

        public string AddonInfoDownloadStretegyName
            => addonInfoDownloadService.CurrentDownloadStretegyName;
    }
}
