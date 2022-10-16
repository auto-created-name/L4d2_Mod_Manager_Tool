using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Core.WorkshopInfoModule;
using Domain.ModFile;
using L4d2_Mod_Manager_Tool.Service.AddonInfoDownload;

namespace L4d2_Mod_Manager_Tool.App
{
    internal class WorkshopInfoApplication
    {
        private ModFileRepository modFileRepository;
        private WorkshopInfoRepository workshopInfoRepository = new();
        private AddonInfoDownloadService addonInfoDownloadService = new();
        public WorkshopInfoApplication(ModFileRepository mfRepo)
        {
            modFileRepository = mfRepo;
        }
        public async Task DownloadWorkshopInfoIfDontHaveAsync()
        {
            var allVpkIds = modFileRepository.GetAll().Where(x => x.VpkId != VpkId.Undefined).Select(x => x.VpkId);
            var savedVpkIds = workshopInfoRepository.GetAll().Select(x => x.Id);
            // 找到未记录的
            var dontHave = allVpkIds.Except(savedVpkIds);
            var res = await addonInfoDownloadService.DownloadAddonInfosAsync(dontHave);
            
            workshopInfoRepository.SaveRange(res);
        }

        public string AddonInfoDownloadStretegyName
            => addonInfoDownloadService.CurrentDownloadStretegyName;
    }
}
