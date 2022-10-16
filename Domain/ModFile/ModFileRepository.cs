using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Infrastructure;
using Infrastructure.Utility;

namespace Domain.ModFile
{
    public delegate void OnModFilesChangedDel(IEnumerable<ModFile> modFile);
    public class ModFileRepository
    {
        private DapperHelper dapperHelper = DapperHelper.Instance;

        public event OnModFilesChangedDel OnModFilesAdded;

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

            OnModFilesAdded?.Invoke(savedList);
            return savedList;
        }

        public void Update(ModFile mf)
        {
            var po = ModFile_DoToPo(mf);
            dapperHelper.Update(po);
            OnModFilesAdded?.Invoke(FPExtension.Pure(mf));
        }

        public ModFile FindById(int id)
        {
            var po = dapperHelper.Get<PO_ModFile>(id);
            return ModFile_PoToDo(po);
        }

        public Maybe<ModFile> FindByVpkId(VpkId vpkId)
        {
            var po = dapperHelper.QuerySingle<PO_ModFile>($"SELECT * FROM mod_file WHERE vpk_id={vpkId.Id}");
            return po switch
            {
                null => Maybe.None,
                PO_ModFile something => ModFile_PoToDo(po)
            };
        }

        public List<ModFile> GetAllNotLocalInfo()
        {
            var pos = dapperHelper.Query<PO_ModFile>("SELECT * FROM mod_file WHERE localinfo_id=0");
            return pos.Select(ModFile_PoToDo).ToList();
        }

        public List<ModFile> GetAll()
        {
            var po = dapperHelper.GetAll<PO_ModFile>();
            return po.Select(ModFile_PoToDo).ToList();
        }
        #region Persistent Object Stuff
        private static PO_ModFile ModFile_DoToPo(ModFile mf)
        {
            if(mf == null)
                return null;

           return new()
               {
                   id = mf.Id,
                   vpk_id = mf.VpkId.Id,
                   file_name = mf.FileLoc,
                   localinfo_id = mf.LocalinfoId,
                   workshopinfo_id = mf.WorkshopinfoId
               };
        }

        private static ModFile ModFile_PoToDo(PO_ModFile po)
        {
            if (po == null)
                return null;
            return new(po.id, new(po.vpk_id), po.file_name, po.localinfo_id, po.workshopinfo_id);
        }
        #endregion
    }
}
