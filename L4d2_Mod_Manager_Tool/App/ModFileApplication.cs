using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Core.ModBriefModule;
using Domain.Core.ModStatusModule;
using Domain.Core.WorkshopInfoModule;
using Domain.ModFile;
using Domain.ModSorter;
using Domain.ModLocalInfo;
using L4d2_Mod_Manager_Tool.Module.FileExplorer;
using ModFile = Domain.ModFile.ModFile;
using ModPreviewInfo = L4d2_Mod_Manager_Tool.Domain.ModPreviewInfo;
using System.Threading;
using L4d2_Mod_Manager_Tool.TaskFramework;

namespace L4d2_Mod_Manager_Tool.App
{
    class ModFileApplication
    {
        public event EventHandler OnModBriefListUpdate;
        private readonly ModFileRepository modFileRepository;
        private readonly LocalInfoRepository localInfoRepository = new();
        private readonly WorkshopInfoRepository workshopInfoRepository = new();
        private readonly ModBriefSpecificationBuilder specBuilder = new();
        private readonly AddonListRepository addonListRepository = new();
        private readonly ModBriefList briefList;
        private readonly BackgroundTaskList backgroundTaskList = new();

        public ModFileApplication(ModFileRepository modFileRepository)
        {
            this.modFileRepository = modFileRepository;
            briefList = new(modFileRepository, localInfoRepository, workshopInfoRepository, addonListRepository);

            // 连接事件
            briefList.OnBriefUpdate += (sender, e) => OnModBriefListUpdate?.Invoke(this, e);
        }

        #region 模组过滤相关
        public void SetModFilter(string name, List<string> tags, List<string> cats)
        {
            specBuilder.SetName(name);
            specBuilder.SetTags(tags);
            specBuilder.SetCategories(cats);
        }
      
        /// <summary>
        /// 获取过滤后的所有模组信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ModBrief> FilteredModInfo()
        {
            var details = briefList.GetSpecified(specBuilder.FinalSpec);
            return modSorter.Sort(details);
        }
        #endregion
        #region 模组状态
        public void EnableMod(int modId)
        {
            var f = modFileRepository.FindById(modId).FileLoc;
            addonListRepository.SetModStatus(f, true);
        }

        public void DisableMod(int modId)
        {
            var f = modFileRepository.FindById(modId).FileLoc;
            addonListRepository.SetModStatus(f, false);
        }

        public bool GetModStatus(int modId)
        {
            var f = modFileRepository.FindById(modId).FileLoc;
            return addonListRepository[f].ValueOr(false);
        }

        public void SaveModStatus() 
            => addonListRepository.Save();
        #endregion
        #region 模组排序
        private IModSorter modSorter = new ModSorter_ByName(ModSortOrder.Ascending);
        public void SetModSortMod(string label, ModSortOrder order)
        {
            modSorter = label switch
            {
                "名称" => new ModSorter_ByName(order),
                "文件名" => new ModSorter_ByFile(order),
                "状态" => new ModSorter_ByEnabled(order),
                "作者" => new ModSorter_ByAuthor(order),
                _ => new ModSorter_Default()
            };
        }
        #endregion
        /// <summary>
        /// 在文件浏览器里查看模组文件
        /// </summary>
        /// <param name="modId"></param>
        public void ShowModFileInFileExplorer(int modId)
        {
            modFileRepository.FindById(modId).ShowFileInExplorer();
        }

        public void OpenModFile(int modId)
        {
            modFileRepository.FindById(modId).OpenModFile();
        }

        /// <summary>
        /// 扫描并保存新模组文件
        /// </summary>
        public void ScanAndSaveNewModFile()
        {
            ModScanner modScanner = new(modFileRepository);
            var modChanged = modScanner.ScanModFileChanged();
            modFileRepository.SaveRange(modChanged.New);
        }

        public ModPreviewInfo? GetModPreview(int modId)
        {
            var mf = modFileRepository.FindById(modId);
            if (mf == null) return null;

            ModPreviewInfo mp = ModPreviewInfo.Default;

            var li = localInfoRepository.FindById(mf.LocalinfoId);
            if (li != null)
            {
                mp.Author = li.Author;
                mp.Categories = Category.Concat(li.Categories);
                mp.Descript = li.Description;
                mp.Name = li.Title;
                mp.PreviewImg = li.AddonImage.File;
            }

            var wi = workshopInfoRepository.GetByVpkId(mf.VpkId);
            wi.Match(wi =>
            {
                mp.Author = wi.Autor;
                mp.Descript = wi.Description;
                mp.Name = wi.Title;
                mp.PreviewImg = wi.Preview.File;
                mp.Tags = Tag.Concat(wi.Tags);
            }, () => { });

            return mp;
        }

        /// <summary>
        /// 【可能需要迁移到其他应用类中】
        /// 分析模组文件的本地信息
        /// </summary>
        public async Task AnalysisModFileLocalInfoIfDontHaveAsync()
        {
            await Task.Run(() =>
            {
                var notLocalInfo = modFileRepository.GetAllNotLocalInfo();
                ModAnalysisServer analysisServer = new();
                //var localInfo = notLocalInfo.AsParallel().Select(mf => (mf, li: analysisServer.AnalysisMod(mf))).Where(x => x.li != null).ToList();

                using var btask = backgroundTaskList.NewTask("解析模组本地信息");
                ConcurrentBag<(ModFile mf, LocalInfo li)> resultBag = new();
                var cts = new CancellationTokenSource();
                int finishedCount = 0, totalCount = notLocalInfo.Count;
                Parallel.For(0, totalCount, new ParallelOptions() { CancellationToken = cts.Token }, x =>
                {
                    var mf = notLocalInfo[x];
                    btask.Status = $"正在解析 {mf.FileLoc}";
                    btask.Progress = finishedCount / (float)totalCount;
                    btask.UpdateProgress();

                    var li = analysisServer.AnalysisMod(mf);
                    if (li != null)
                        resultBag.Add((mf, li));
                    ++finishedCount;
                });
                //localInfo.ForEach(x => SaveLocalInfoAndUpdateModFile(x.mf, x.li));
                resultBag.ToList().ForEach(x => SaveLocalInfoAndUpdateModFile(x.mf, x.li));
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 保存LocalInfo，同时更新ModFile
        /// </summary>
        private void SaveLocalInfoAndUpdateModFile(ModFile mf, LocalInfo li)
        {
            var savedLi = localInfoRepository.Save(li);
            var newMf = mf with { LocalinfoId = savedLi.Id };
            modFileRepository.Update(newMf);
        }
    }
}
