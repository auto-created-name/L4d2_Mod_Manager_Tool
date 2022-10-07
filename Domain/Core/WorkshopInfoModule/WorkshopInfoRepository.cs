using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Domain.Core.WorkshopInfoModule
{
    public class WorkshopInfoRepository
    {
        private DapperHelper dapperHelper = DapperHelper.Instance;


        public WorkshopInfo[] GetAll() 
        { 
            var pos = dapperHelper.GetAll<PO_WorkshopInfo>();
            return pos.Select(WorkshopInfo_POToDo).ToArray();
        }

        public void SaveRange(IEnumerable<WorkshopInfo> xs)
        {
            using var conn = DapperHelper.OpenConnection();
            using var trans = conn.BeginTransaction();

            var pos = xs.Select(WorkshopInfo_DoToPo).ToList();//.ForEach(x => trans.Execute("INSERT INTO workshopinfo VALUES(@vpk_id, @preview, @title, @description", x));
            trans.Execute("INSERT INTO workshopinfo VALUES(@vpk_id, @preview, @title, @description, @tags)", pos);
        }

        private static WorkshopInfo WorkshopInfo_POToDo(PO_WorkshopInfo po)
            => new(new(po.vpk_id))
            {
                Description = po.description,
                Preview = new ImageFile(po.preview),
                Tags = Tag.ParseGroup(po.tags),
                Title = po.title
            };

        private static PO_WorkshopInfo WorkshopInfo_DoToPo(WorkshopInfo d)
            => new()
            {
                description = d.Description,
                preview = d.Preview.File,
                tags = Tag.Concat(d.Tags),
                title = d.Title,
                vpk_id = d.Id.Id
            };
    }
}
