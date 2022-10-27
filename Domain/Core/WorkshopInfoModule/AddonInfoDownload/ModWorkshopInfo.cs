using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.WorkshopInfoModule.AddonInfoDownload
{
    /// <summary>
    /// 模组的创意工坊信息
    /// </summary>
    public record ModWorkshopInfo(
        string Author,
        string Title, 
        string Descript, 
        string PreviewImage,
        ImmutableArray<string> Tags
        )
    {
        public bool IsEmpty
            => string.IsNullOrEmpty(Author) 
            && string.IsNullOrEmpty(Title) 
            && string.IsNullOrEmpty(Descript) 
            && string.IsNullOrEmpty(PreviewImage) 
            && Tags.IsEmpty;
    }
}
