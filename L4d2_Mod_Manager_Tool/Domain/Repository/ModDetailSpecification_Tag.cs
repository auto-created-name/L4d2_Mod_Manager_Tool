using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    class ModDetailSpecification_Tag : ISpecification<ModDetail>
    {
        private string tag;
        public ModDetailSpecification_Tag(string tag)
        {
            this.tag = tag;
        }

        public string ToSqlite()
            => $"workshop_tags like '%{tag}%'";
    }
}
