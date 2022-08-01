using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.ModSorter
{
    class ModSorter_ByEnabled : IModSorter
    {
        private ModSortOrder order;
        public ModSorter_ByEnabled(ModSortOrder order)
        {
            this.order = order;
        }
        public IEnumerable<ModDetail> Sort(IEnumerable<ModDetail> mods)
        {
            return order switch
            {
                ModSortOrder.Ascending => mods.OrderBy(m => m.Enabled),
                ModSortOrder.Descending => mods.OrderByDescending(m => m.Enabled)
            };
        }
    }
}
