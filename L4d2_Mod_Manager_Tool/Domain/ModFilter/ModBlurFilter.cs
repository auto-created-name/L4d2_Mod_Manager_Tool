using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.ModFilter
{
    class ModBlurFilter : IModFilter
    {
        private string name;
        public ModBlurFilter(string name)
        {
            this.name = name;
        }
        public IEnumerable<Mod> FilterMod(IEnumerable<Mod> mods)
        {
            var filter =
                new PredicateModFilter(ModFP.NameContains(name))
                .Or(new PredicateModFilter(ModFP.AuthorContains(name)))
                .Or(new PredicateModFilter(ModFP.VPKIdContains(name)));
            return filter.FilterMod(mods);
        }
    }
}
