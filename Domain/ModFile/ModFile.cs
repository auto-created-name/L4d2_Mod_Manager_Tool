using Domain.Core;
using Infrastructure.Utility;

namespace Domain.ModFile
{
    /// <summary>
    /// 模组文件实体
    /// </summary>
    public record ModFile(
        int Id,
        VpkId VpkId,
        string FileLoc,
        int LocalinfoId
    ){
        public static ModFile CreateFromFile(string file)
            => new(0, VpkId.TryParse(file), file, 0);

        public void ShowFileInExplorer()
        {
            var file = L4d2Folder.GetAddonFileFullPath(FileLoc);
            FileExplorerUtils.OpenFileExplorerAndSelectItem(file);
        }

        public void OpenModFile()
        {
            var file = L4d2Folder.GetAddonFileFullPath(FileLoc);
            FileExplorerUtils.OpenFileInExplorer(file);
        }

        /// <summary>
        /// 模组文件是否存在
        /// </summary>
        /// <returns>文件如果存在返回true，否则返回false</returns>
        public bool ModExist()
        {
            string absFile = L4d2Folder.GetAddonFileFullPath(FileLoc);
            return System.IO.File.Exists(absFile);
        }
    };
}
