using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    class ModDetailSpecification_Category : ISpecification<ModDetail>
    {
        private string category;
        public ModDetailSpecification_Category(string category)
        {
            this.category = category;
        }
        public string ToSqlite()
        {
            return $"categories like '%{category}%'";
        }
    }
}
