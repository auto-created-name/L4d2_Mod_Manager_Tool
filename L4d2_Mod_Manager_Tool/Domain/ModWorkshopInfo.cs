using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain
{
    /// <summary>
    /// 模组的创意工坊信息
    /// </summary>
    public record ModWorkshopInfo(
        string Title, 
        string Descript, 
        string PreviewImage,
        ImmutableArray<string> Tags
        );
}
