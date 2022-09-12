using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    };
}
