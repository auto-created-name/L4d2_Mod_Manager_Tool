using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.ModFilter
{
    class AndFilter : IModFilter
    {
        private IModFilter filter1, filter2;
        public AndFilter(IModFilter filter1, IModFilter filter2)
        {
            this.filter1 = filter1;
            this.filter2 = filter2;
        }
        public IEnumerable<Mod> FilterMod(IEnumerable<Mod> mods)
        {
            return filter1.FilterMod(mods).Intersect(filter2.FilterMod(mods), new ModEqualityComparer());
        }
    }
}
