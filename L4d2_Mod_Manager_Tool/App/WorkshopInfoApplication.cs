using Domain.Core;
using Domain.Core.WorkshopInfoModule;
using Domain.Core.WorkshopInfoModule.AddonInfoDownload;
using Domain.ModFile;
using L4d2_Mod_Manager_Tool.TaskFramework;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.App
{
    internal class WorkshopInfoApplication
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

        public string AddonInfoDownloadStretegyName
            => addonInfoDownloadService.CurrentDownloadStretegyName;
    }
}
