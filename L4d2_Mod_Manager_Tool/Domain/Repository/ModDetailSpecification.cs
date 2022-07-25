using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    class ModDetailSpecification : ISpecification<ModDetail>
    {
        private string prop;
        public ModDetailSpecification(string prop)
        {
            this.prop = prop;
        }

        public bool IsSatisifiedBy(ModDetail o)
        {
            throw new NotImplementedException();
        }
    }
}
