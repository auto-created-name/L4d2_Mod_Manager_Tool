using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Domain.ModFile
{
    public class ModFileRepository
    {
        private DapperHelper dapperHelper = DapperHelper.Instance;

        public List<ModFile> SaveRange(IEnumerable<ModFile> mfs)
        {
            var conn = DapperHelper.OpenConnection();
            var tran = conn.BeginTransaction();

            var savedList = mfs.ToList().Select(mf =>
            {
                var po = ModFile_DoToPo(mf);
                var id = tran.Insert(po);
                return mf with { Id = (int)id };
            }).ToList();

            tran.Dispose();
            conn.Dispose();
            return savedList;
        }

        public List<ModFile> GetAll()
        {
            var po = dapperHelper.GetAll<PO_ModFile>();
            return po.Select(ModFile_PoToDo).ToList();
        }

        private static PO_ModFile ModFile_DoToPo(ModFile mf)
            => new() { 
                id = mf.Id, 
                vpk_id = mf.VpkId.Id, 
                file_name = mf.FileLoc,
                localinfo_id = mf.LocalinfoId, 
                workshopinfo_id = mf.WorkshopinfoId 
            };

        private static ModFile ModFile_PoToDo(PO_ModFile po)
            => new(po.id, new(po.vpk_id), po.file_name, po.localinfo_id, po.workshopinfo_id);
    }
}
