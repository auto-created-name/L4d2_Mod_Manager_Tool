using Domain.Core.ModStatusModule;
using Domain.ModFile;
using Domain.ModLocalInfo;
using Domain.Core.WorkshopInfoModule;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Core.ModBriefModule
{
    /// <summary>
    /// 模组摘要信息列表
    /// </summary>
    public class ModBriefList
    {
        private Dictionary<int, ModBrief> modDetails = new();

        public ModBriefList(ModFileRepository mfRepo, LocalInfoRepository liRepo, WorkshopInfoRepository wiRepo, AddonListRepository alRepo)
        {
            mfRepo.GetAll().ForEach(mf => 
            {
                ModBrief md = new(mf.Id)
                { 
                    FileName = mf.FileLoc, 
                    VpkId = mf.VpkId, 
                    Enabled = alRepo[mf.FileLoc].ValueOr(false)
                };

                if(mf.LocalinfoId > 0)
                {
                    var li = liRepo.FindById(mf.LocalinfoId);
                    md.Author = li.Author;
                    md.Name = li.Title;
                    md.Tagline = li.Tagline;
                    md.Categories = Category.Concat(li.Categories);
                }

                wiRepo.GetByVpkId(mf.VpkId).Match(wi => 
                {
                    md.Name = string.IsNullOrEmpty(wi.Title) ? md.Name : wi.Title;
                    md.Author = string.IsNullOrEmpty(wi.Autor) ? md.Author : wi.Autor;
                    md.Tags = Tag.Concat(wi.Tags);
                }, () => { });
                modDetails.Add(md.Id, md);
            });
        }

        public ModBrief[] GetSpecified(ModBriefSpecification spec)
        {
            return modDetails.Values.Where(x => spec.IsSpecified(x)).ToArray();
        }
    }
}
