using System.IO;

namespace L4d2_Mod_Manager_Tool.Domain
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
