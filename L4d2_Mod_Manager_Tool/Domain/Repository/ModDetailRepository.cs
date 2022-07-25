using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    public class ModDetailRepository : SQLiteDatabaseRepositoryBase
    {
        public ModDetailRepository() : base("mod.db") { }
        protected override void CreateDatabase() { }

        //public Maybe<ModDetail> FindOne(ISpecification<ModDetail> spec)
        //{
        //    var head = "SELECT * FROM mod INNER JOIN mod_file";
        //}
    }
}
