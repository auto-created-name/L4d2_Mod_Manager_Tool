using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ModFile;
using Domain.ModLocalInfo;

namespace L4d2_Mod_Manager_Tool.App
{
    class ModFileApplication
    {
        private readonly ModFileRepository modFileRepository = new();
        private readonly LocalInfoRepository localInfoRepository = new();

        /// <summary>
        /// 扫描并保存新模组文件
        /// </summary>
        public void ScanAndSaveNewModFile()
        {
            ModScanner modScanner = new(modFileRepository);
            var modChanged = modScanner.ScanModFileChanged();
            modFileRepository.SaveRange(modChanged.New);
        }

        /// <summary>
        /// 【可能需要迁移到其他应用类中】
        /// 分析模组文件的本地信息
        /// </summary>
        public void AnalysisModFileLocalInfoIfDontHave()
        {
            var notLocalInfo = modFileRepository.GetAllNotLocalInfo();
            ModAnalysisServer analysisServer = new();
            var localInfo = notLocalInfo.AsParallel().Select(mf => (mf, li:analysisServer.AnalysisMod(mf))).Where(x => x.li != null).ToList();
            localInfo.ForEach(x => localInfoRepository.Save(x.li));
            //TODO: 储存后的localinfo id 需要更新到 modfile 记录中
        }
    }
}
