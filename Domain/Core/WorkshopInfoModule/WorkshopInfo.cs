using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.WorkshopInfoModule
{
    /// <summary>
    /// 创意工坊信息
    /// </summary>

    public class WorkshopInfo : Entity<VpkId>
    {
        public WorkshopInfo(VpkId id) : base(id) { }

        public ImageFile Preview { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Tag[] Tags { get; set; }

        public static WorkshopInfo CreateEmpty(VpkId id)
        {
            return new WorkshopInfo(id)
            {
                Preview = ImageFile.MissingImage,
                Title = string.Empty,
                Description = string.Empty,
                Tags = Array.Empty<Tag>()
            };
        }
    }
}
