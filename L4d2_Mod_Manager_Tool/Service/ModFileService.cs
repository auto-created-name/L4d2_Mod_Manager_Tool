using L4d2_Mod_Manager_Tool.Domain;
using L4d2_Mod_Manager_Tool.Domain.Repository;
using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    }
}
