using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    public class ModDetailQuery : SQLiteDatabaseRepositoryBase
    {
        private AddonListRepository addonListRepo;
        public ModDetailQuery(AddonListRepository addonListRepo) : base("mod.db") 
        {
            this.addonListRepo = addonListRepo;
        }
        protected override void CreateDatabase() { }

        public Maybe<ModDetail> FindOne(ISpecification<ModDetail> spec)
        {
            var sql = "SELECT * FROM mod INNER JOIN mod_file ON mod.file_id = mod_file.id where " + spec.ToSqlite();
            using var reader = ExecuteQueryReader(sql);
            if (reader.Read())
                return FromReader(reader);
            else
                return Maybe.None;
        }

        public IEnumerable<ModDetail> FindAll(ISpecification<ModDetail> spec)
        {
            var sql = "SELECT * FROM mod INNER JOIN mod_file ON mod.file_id = mod_file.id";
            if (spec is not EmptySpecification<ModDetail>)
                sql += " where " + spec.ToSqlite();
            using var reader = ExecuteQueryReader(sql);
            while (reader.Read())
                yield return FromReader(reader);
        }

        private ModDetail FromReader(SQLiteDataReader reader)
        {
            int id = reader.GetInt32(0);
            bool enabled = addonListRepo.ModEnabled(id).ValueOr(true);
            return new ModDetail(
                    id
                    , IfEmptyReturn(reader["title"].ToString(), "<无名称>")
                    , reader["filename"].ToString()
                    , enabled
                    , IfEmptyReturn(reader["author"].ToString(), "<未知作者>")
                    , IfEmptyReturn(reader["tagline"].ToString(), "<无简介>")
                );
        }

        private string IfEmptyReturn(string ori, string def)
        {
            return string.IsNullOrEmpty(ori) ? def : ori;
        }
    }
}
