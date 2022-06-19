﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager.Domain
{
    public record ModFile(int Id, string FilePath, bool Actived);

    public static class ModFileFP
    {
        /// <summary>
        /// 创建一个新的模组文件
        /// </summary>
        public static ModFile CreateModFile(string file)
        {
            return new ModFile(-1, file, true);
        }

        public static string ModFileName(ModFile mf)
        {
            return Path.GetFileNameWithoutExtension(mf.FilePath);
        }
    }
}
