using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModLocalInfo
{
    public class LocalInfoRepository
    {
        private DapperHelper dapperHelper = DapperHelper.Instance;

        public LocalInfo Save(LocalInfo li)
        {
            var po = LocalInfo_DoToPo(li);
            int id = (int)dapperHelper.Insert(po);
            return li.WithId(id);
        }

        private PO_LocalInfo LocalInfo_DoToPo(LocalInfo li)
        {
            return new PO_LocalInfo()
            {
                id = li.Id,
                author = li.Author,
                categories = string.Join(',', li.Categories.Select(cat => cat.Name)),
                description = li.Description,
                tagline = li.Tagline,
                thumbnail = li.AddonImage.File,
                title = li.Title,
                version = li.Version
            };
        }
    }
}
