using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Core.ModBriefModule;
using Domain.Core.ModStatusModule;
using Domain.ModFile;
using Domain.ModLocalInfo;
using L4d2_Mod_Manager_Tool.Module.FileExplorer;
using ModFile = Domain.ModFile.ModFile;
using ModPreviewInfo = L4d2_Mod_Manager_Tool.Domain.ModPreviewInfo;

namespace L4d2_Mod_Manager_Tool.App
{
    class ModFileApplication
    {
        private readonly ModFileRepository modFileRepository = new();
        private readonly LocalInfoRepository localInfoRepository = new();
        private readonly ModBriefSpecificationBuilder specBuilder = new();
        private readonly AddonListRepository addonListRepository = new();
        private readonly ModBriefList briefList;

        public ModFileApplication()
        {
            briefList = new(modFileRepository, localInfoRepository, addonListRepository);
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
        public IEnumerable<ModDetail> FilteredModInfo()
        {
            return briefList.GetSpecified(specBuilder.FinalSpec);
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

        /// <summary>
        /// 在文件浏览器里查看模组文件
        /// </summary>
        /// <param name="modId"></param>
        public void ShowModFileInFileExplorer(int modId)
        {
            var file = L4d2Folder.GetAddonFileFullPath(modFileRepository.FindById(modId).FileLoc);
            FileExplorerUtils.OpenFileExplorerAndSelectItem(file);
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

            var li = localInfoRepository.FindById(mf.LocalinfoId);
            if (li != null)
            {
                ModPreviewInfo mp = new();
                mp.Author = li.Author;
                mp.Name = li.Title;
                mp.Descript = li.Description;
                mp.Categories = Category.Concat(li.Categories);
                mp.PreviewImg = li.AddonImage.File;
                return mp;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 【可能需要迁移到其他应用类中】
        /// 分析模组文件的本地信息
        /// </summary>
        public void AnalysisModFileLocalInfoIfDontHave()
        {
            var notLocalInfo = modFileRepository.GetAllNotLocalInfo();
            ModAnalysisServer analysisServer = new();
            var localInfo = notLocalInfo.AsParallel().Select(mf => (mf, li: analysisServer.AnalysisMod(mf))).Where(x => x.li != null).ToList();
            localInfo.ForEach(x => SaveLocalInfoAndUpdateModFile(x.mf, x.li));
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
