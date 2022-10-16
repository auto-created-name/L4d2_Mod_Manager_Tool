using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModSorter
{
    public class ModSorter_ByEnabled : IModSorter
    {
        private ModSortOrder order;
        public ModSorter_ByEnabled(ModSortOrder order)
        {
            this.order = order;
        }
        public IEnumerable<ModBrief> Sort(IEnumerable<ModBrief> mods)
        {
            return order switch
            {
                ModSortOrder.Ascending => mods.OrderBy(m => m.Enabled),
                ModSortOrder.Descending => mods.OrderByDescending(m => m.Enabled)
            };
        }
    }
}
