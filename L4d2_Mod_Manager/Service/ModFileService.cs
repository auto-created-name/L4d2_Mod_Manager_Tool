﻿using L4d2_Mod_Manager.Domain;
using L4d2_Mod_Manager.Domain.Repository;
using L4d2_Mod_Manager.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager.Service
{
    public static class ModFileService
    {
        private static ModFileRepository modFileRepo = new();

        public static Maybe<ModFile> FindFileById(int fid)
        {
            return modFileRepo.FindModFileById(fid);
        }

        /// <summary>
        /// [副作用]修改数据库和文件，将模组文件关闭
        /// </summary>
        public static ModFile DeactiveModFile(ModFile mf)
        {
            if (mf.Actived)
            {
                // 更新数据库
                string deactiveFilePath = mf.FilePath + ".deactive";
                var newMf = mf with { Actived = false, FilePath = deactiveFilePath };
                modFileRepo.UpdateModFile(newMf);

                // 更新文件
                File.Move(mf.FilePath, deactiveFilePath);
                return newMf;
            }
            else
            {
                return mf;
            }
        }
    }
}
