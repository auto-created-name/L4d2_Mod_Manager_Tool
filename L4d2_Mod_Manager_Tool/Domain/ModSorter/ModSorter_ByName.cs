using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.ModSorter
{
    class ModSorter_ByName : IModSorter
    {
        private ModSortOrder order;
        public ModSorter_ByName(ModSortOrder order)
        {
            this.order = order;
        }
        public IEnumerable<Mod> Sort(IEnumerable<Mod> mods)
        {
            var ord = mods.OrderBy(ModFP.SelectName, StringComparer.CurrentCultureIgnoreCase);
            return order switch
            {
                ModSortOrder.Ascending => ord,
                ModSortOrder.Descending => ord.Reverse(),
                _ => ord
            };
        }
    }
}
