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

        private static WorkshopInfo WorkshopInfo_POToDo(PO_WorkshopInfo po)
            => new(new(po.vpk_id))
            {
                Description = po.description,
                Preview = new ImageFile(po.preview),
                Tags = Tag.ParseGroup(po.tags),
                Title = po.title
            };
    }
}
