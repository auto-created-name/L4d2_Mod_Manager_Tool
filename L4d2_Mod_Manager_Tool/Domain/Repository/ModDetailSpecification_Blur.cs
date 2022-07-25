using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    class ModDetailSpecification_Blur : ISpecification<ModDetail>
    {
        private string blur;
        public ModDetailSpecification_Blur(string blur)
        {
            this.blur = blur;
        }
        public string ToSqlite()
        {
            return string.Format(
                "(title like '%{0}%' OR workshop_title like '%{0}%' OR author like '%{0}%' OR vpk_id like '{0}%')",
                blur);
        }
    }
}
