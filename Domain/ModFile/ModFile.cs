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
        int LocalinfoId,
        int WorkshopinfoId
    ){
        public static ModFile CreateFromFile(string file)
            => new(0, VpkId.TryParse(file), file, 0, 0);

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
    };
}
