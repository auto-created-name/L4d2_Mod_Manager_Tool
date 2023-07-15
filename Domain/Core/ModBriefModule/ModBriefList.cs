using Domain.Core.ModStatusModule;
using Domain.ModFile;
using Domain.ModLocalInfo;
using Domain.Core.WorkshopInfoModule;
using System.Collections.Generic;
using System.Linq;
using System;
using Infrastructure.Utility;

namespace Domain.Core.ModBriefModule
{
    /// <summary>
    /// 模组摘要信息列表
    /// </summary>
    public class ModBriefList
    {
        /// <summary>
        /// 模组摘要更新
        /// </summary>
        public event EventHandler OnBriefUpdate;

        private Dictionary<int, ModBrief> modDetails = new();
        private ModFileRepository mfRepo;
        private LocalInfoRepository liRepo;
        private WorkshopInfoRepository wiRepo;
        private AddonListRepository alRepo;

        public ModBriefList(ModFileRepository mfRepo, LocalInfoRepository liRepo, WorkshopInfoRepository wiRepo, AddonListRepository alRepo)
        {
            this.mfRepo = mfRepo;
            this.liRepo = liRepo;
            this.wiRepo = wiRepo;
            this.alRepo = alRepo;

            mfRepo.OnModFilesAdded += ModFileRepository_OnModFilesAdded;
            mfRepo.OnModFilesLosted += ModFileRepository_OnModFilesLosted;
            // localinfo更新会触发mf更新，不需要单独观察更改（除非考虑li单独更新，但是可以用li删除+重置)
            //liRepo.OnLocalInfosAdded += LocalInfoRepository_OnLocalInfosAdded;
            wiRepo.OnWorkshopInfoAdded += WorkshopInfoRepository_OnWorkshopInfoAdded;

            mfRepo.GetAll().ForEach(UpdateModFileInfo);
        }

        /// <summary>
        /// 更新所有的模组摘要信息
        /// </summary>
        public void UpdateAll()
        {
            modDetails.Clear();
            mfRepo.GetAll().ForEach(UpdateModFileInfo);
        }

        // 当【创意工坊信息】更新时，更新相关视图对象
        private void WorkshopInfoRepository_OnWorkshopInfoAdded(IEnumerable<WorkshopInfo> workshopInfos)
        {
            workshopInfos.Select(wi => mfRepo.FindByVpkId(wi.Id)).ToList()
                .ForEach(mf => mf.Map(UpdateModFileInfo));
            OnBriefUpdate?.Invoke(this, null);
        }

        // 当【有新模组文件】时，更新相关视图对象
        private void ModFileRepository_OnModFilesAdded(IEnumerable<ModFile.ModFile> mfs)
        {
            mfs.ToList().ForEach(UpdateModFileInfo);
            OnBriefUpdate?.Invoke(this, null);
        }

        // 当【丢失模组文件】时，更新相关视图对象
        private void ModFileRepository_OnModFilesLosted(IEnumerable<ModFile.ModFile> mfs)
        {
            mfs.ToList().ForEach(UpdateModFileInfo);
            OnBriefUpdate?.Invoke(this, null);
        }

        /// <summary>
        /// 更新指定ModFile视图
        /// </summary>
        private void UpdateModFileInfo(ModFile.ModFile mf)
        {
            ModBrief md = new(mf.Id)
            {
                ModExisted = mf.ModExist(),
                FileName = mf.FileLoc,
                VpkId = mf.VpkId,
                Enabled = alRepo[mf.FileLoc].ValueOr(false)
            };

            if (mf.LocalinfoId > 0)
            {
                var li = liRepo.FindById(mf.LocalinfoId);
                if (li != null)
                {
                    md.Author = li.Author;
                    md.Name = li.Title;
                    md.Tagline = li.Tagline;
                    md.Categories = Category.Concat(li.Categories);
                }
            }

            wiRepo.GetByVpkId(mf.VpkId).Match(wi =>
            {
                md.Name = string.IsNullOrEmpty(wi.Title) ? md.Name : wi.Title;
                md.Author = string.IsNullOrEmpty(wi.Autor) ? md.Author : wi.Autor;
                md.Tags = Tag.Concat(wi.Tags);
            }, () => { });
            modDetails[md.Id] = md;
        }

        public ModBrief[] GetSpecified(ModBriefSpecification spec)
        {
            return modDetails.Values.Where(x => spec.IsSpecified(x)).ToArray();
        }

        public Maybe<ModBrief> GetById(int id)
            => modDetails.ContainsKey(id) ? modDetails[id] : Maybe.None;
    }
}
