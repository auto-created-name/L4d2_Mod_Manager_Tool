using Domain.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModFile
{
    public class ModScanner
    {
        private readonly ModFileRepository modFileRepository;

        public ModScanner(ModFileRepository modFileRepository)
        {
            this.modFileRepository = modFileRepository;
        }

        /// <summary>
        /// 扫描所有的模组文件，找到与记录的变化
        /// </summary>
        public ModFileChanged ScanModFileChanged()
        {
            var savedMods = modFileRepository.GetAll();

            var localAddons = ListVpk(L4d2Folder.AddonsFolder);
            var workshopAddons = ListVpk(L4d2Folder.WorkshopAddonsFolder).Select(x => $"workshop\\{x}");
            var addons = localAddons.Concat(workshopAddons).ToArray();

            var newAddons = addons.Except(savedMods.Select(m => m.FileLoc));
            var lostMods = savedMods.Where(m => !addons.Contains(m.FileLoc));

            return new(lostMods.ToArray(), newAddons.Select(ModFile.CreateFromFile).ToArray());
        }

        /// <summary>
        /// 列出指定文件夹中所有vpk文件
        /// </summary>
        private static IEnumerable<string> ListVpk(string folder)
        {
            DirectoryInfo di = new(folder);
            if (di.Exists)
            {
                var res = di.GetFiles("*.vpk").Select(f => f.Name).ToArray();
                return res;
            }
            else
            {
                return Array.Empty<string>();
            }
        }
    }
}
