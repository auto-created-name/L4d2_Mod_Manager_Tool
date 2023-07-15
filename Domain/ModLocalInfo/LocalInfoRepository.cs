using Infrastructure;
using Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModLocalInfo
{
    public delegate void LocalInfosDel(IEnumerable<LocalInfo> localInfos);
    public class LocalInfoRepository
    {
        private DapperHelper dapperHelper = DapperHelper.Instance;
        public event LocalInfosDel OnLocalInfosAdded;

        public LocalInfo Save(LocalInfo li)
        {
            var po = LocalInfo_DoToPo(li);
            int id = (int)dapperHelper.Insert(po);
            var resLi = li.WithId(id);
            OnLocalInfosAdded?.Invoke(FPExtension.Pure(resLi));
            return resLi;
        }

        /// <summary>
        /// 删除本地信息
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteById(IEnumerable<int> ids)
        {
            var idList = ids.Where(id => id > 0).ToList();
            if (idList.Count == 0)
                return;

            using var trans = DapperHelper.OpenConnection().BeginTransaction();
            foreach (var id in ids)
            {
                trans.Execute($"DELETE FROM mod WHERE id={id}");
            }
        }

        public LocalInfo FindById(int id)
        {
            var po = dapperHelper.Get<PO_LocalInfo>(id);
            return LocalInfo_PoToDo(po);
        }

        private PO_LocalInfo LocalInfo_DoToPo(LocalInfo li)
        {
            if (li == null) return null;

            return new PO_LocalInfo()
            {
                id = li.Id,
                author = li.Author,
                categories = Category.Concat(li.Categories),
                description = li.Description,
                tagline = li.Tagline,
                thumbnail = li.AddonImage.File,
                title = li.Title,
                version = li.Version
            };
        }

        private LocalInfo LocalInfo_PoToDo(PO_LocalInfo po)
        {
            if(po == null) return null;

            return new LocalInfo(po.id)
            {
                AddonImage = new(po.thumbnail),
                Author = po.author,
                Categories = Category.ParseGroup(po.categories),
                Description = po.description,
                Tagline = po.tagline,
                Title = po.title,
                Version = po.version
            };
        }
    }
}
