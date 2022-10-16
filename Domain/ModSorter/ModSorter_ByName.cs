using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModSorter
{
    public class ModSorter_ByName : IModSorter
    {
        private ModSortOrder order;
        public ModSorter_ByName(ModSortOrder order)
        {
            this.order = order;
        }
        public IEnumerable<ModBrief> Sort(IEnumerable<ModBrief> mods)
        {
            var ord = mods.OrderBy(m => m.Name, StringComparer.CurrentCultureIgnoreCase);
            return order switch
            {
                ModSortOrder.Ascending => ord,
                ModSortOrder.Descending => ord.Reverse(),
                _ => ord
            };
        }
    }
}
