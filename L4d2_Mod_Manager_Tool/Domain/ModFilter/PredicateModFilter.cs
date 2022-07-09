using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.ModFilter
{
    class PredicateModFilter : IModFilter
    {
        private Func<Mod, bool> predicate;
        public PredicateModFilter(Func<Mod, bool> predicate)
        {
            this.predicate = predicate ?? throw new Exception("谓词不能为空");
        }
        public IEnumerable<Mod> FilterMod(IEnumerable<Mod> mods)
        {
            return mods.Where(predicate);
        }
    }
}
