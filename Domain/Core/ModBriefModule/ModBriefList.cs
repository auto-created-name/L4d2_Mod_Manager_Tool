﻿using Domain.Core.ModStatusModule;
using Domain.ModFile;
using Domain.ModLocalInfo;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Core.ModBriefModule
{
    /// <summary>
    /// 模组摘要信息列表
    /// </summary>
    public class ModBriefList
    {
        private Dictionary<int, ModDetail> modDetails = new();

        public ModBriefList(ModFileRepository mfRepo, LocalInfoRepository liRepo, AddonListRepository alRepo)
        {
            mfRepo.GetAll().ForEach(mf => 
            {
                ModDetail md = ModDetail.Default with 
                { 
                    Id = mf.Id, 
                    FileName = mf.FileLoc, 
                    VpkId = mf.VpkId, 
                    Enabled = alRepo[mf.FileLoc].ValueOr(false)
                };

                if(mf.LocalinfoId > 0)
                {
                    var li = liRepo.FindById(mf.LocalinfoId);
                    md = md with
                    {
                        Author = li.Author, 
                        Name = li.Title, 
                        Tagline = li.Tagline, 
                        Categories = Category.Concat(li.Categories) 
                    };
                }
                modDetails.Add(md.Id, md);
            });
        }

        public ModDetail[] GetSpecified(ModBriefSpecification spec)
        {
            return modDetails.Values.Where(x => spec.IsSpecified(x)).ToArray();
        }
    }
}
