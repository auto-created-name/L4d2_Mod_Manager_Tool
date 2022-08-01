using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.ModSorter
{
    class ModSorter_ByAuthor : IModSorter
    {
        private ModSortOrder order;
        public ModSorter_ByAuthor(ModSortOrder order)
        {
            this.order = order;
        }
        public IEnumerable<ModDetail> Sort(IEnumerable<ModDetail> mods)
        {
            var ord = mods.OrderBy(m => m.Author, StringComparer.CurrentCultureIgnoreCase);
            return order switch
            {
                ModSortOrder.Ascending => ord,
                ModSortOrder.Descending => ord.Reverse(),
                _ => ord
            };
        }
    }
}
