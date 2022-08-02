using L4d2_Mod_Manager_Tool.Domain;
using L4d2_Mod_Manager_Tool.Domain.Repository;
using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Service
{
    public static class ModFileService
    {
        private static ModFileRepository modFileRepo = new();

        public static Maybe<ModFile> FindFileById(int fid)
        {
            return modFileRepo.FindModFileById(fid);
        }

        public static Maybe<ModFile> FindFileByFileName(string fn)
        {
            return modFileRepo.FindModFileByFileName(fn);
        }

        public static ModFile SaveModFile(ModFile mf)
        {
            return modFileRepo.SaveModFile(mf);
        }

        public static bool ModFileExists(string file)
        {
            return modFileRepo.ModFileExists(file);
        }

        /// <summary>
        /// 开始扫描模组文件
        /// </summary>
        public static void BeginScanModFile(IProgress<float> reporter)
        {
            // 扫描所有本地模组，如果模组还未入系统，则解压并读取信息，存入数据库
            // 构建解压任务集
            // 推入后台任务系统
            var mfs = VPKServices.ScanAllModFile().ToArray();
            CancellationTokenSource source = new();
            //TaskFramework.BackgroundWorks.Instance.AppendTasks("扫描模组信息", mfs.Select(ExtraMod).ToArray());
            ExtraMods(mfs, reporter, source.Token);
        }

        static async Task ExtraMods(ModFile[] modFiles, IProgress<float> reporter, CancellationToken token)
        {
            await Task.Run(() => 
            {
                int total = modFiles.Length;
                int finished = 0;
                var tmpMods = modFiles.AsParallel().Select(mf =>
                {
                    if (token.IsCancellationRequested)
                        return null;
                    var tmpMod = ExtraMod(mf);
                    reporter.Report(++finished / (float)total);
                    return tmpMod;
                }).ToArray();
                //批量储存到数据库
                ModRepository.Instance.SaveRange(tmpMods);
                reporter.Report(1);
            }, token);
        }

        static Mod ExtraMod(ModFile modFile)
        {
            var savedMf = ModFileService.SaveModFile(modFile);
            return VPKServices.ExtraMod(savedMf);
        }
    }
}
