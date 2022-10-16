using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Utility;

namespace Domain.Core.WorkshopInfoModule
{
    public delegate void WorkshopInfoDel(IEnumerable<WorkshopInfo> workshopInfos);
    public class WorkshopInfoRepository
    {
        private DapperHelper dapperHelper = DapperHelper.Instance;

        public event WorkshopInfoDel OnWorkshopInfoAdded;

        public WorkshopInfo[] GetAll() 
        { 
            var pos = dapperHelper.GetAll<PO_WorkshopInfo>();
            return pos.Select(WorkshopInfo_PoToDo).ToArray();
        }

        public Maybe<WorkshopInfo> GetByVpkId(VpkId id)
        {
            var po = dapperHelper.Get<PO_WorkshopInfo>(id.Id);
            return po switch
            {
                null => Maybe.None,
                _ => WorkshopInfo_PoToDo(po)
            };
        }

        public void SaveRange(IEnumerable<WorkshopInfo> xs)
        {
            using var conn = DapperHelper.OpenConnection();
            using var trans = conn.BeginTransaction();

            var pos = xs.Select(WorkshopInfo_DoToPo).ToList();//.ForEach(x => trans.Execute("INSERT INTO workshopinfo VALUES(@vpk_id, @preview, @title, @description", x));
            trans.Execute("INSERT INTO workshopinfo VALUES(@vpk_id, @author, @preview, @title, @description, @tags)", pos);
            OnWorkshopInfoAdded?.Invoke(xs);
        }

        private static WorkshopInfo WorkshopInfo_PoToDo(PO_WorkshopInfo po)
            => new(new(po.vpk_id))
            {
                Description = po.description,
                Preview = new ImageFile(po.preview),
                Tags = Tag.ParseGroup(po.tags),
                Title = po.title,
                Autor = po.author
            };

        private static PO_WorkshopInfo WorkshopInfo_DoToPo(WorkshopInfo d)
            => new()
            {
                description = d.Description,
                preview = d.Preview.File,
                tags = Tag.Concat(d.Tags),
                title = d.Title,
                vpk_id = d.Id.Id,
                author = d.Autor
            };
    }
}
