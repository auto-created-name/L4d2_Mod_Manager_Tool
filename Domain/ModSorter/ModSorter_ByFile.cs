using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModSorter
{
    public class ModSorter_ByFile : IModSorter
    {
        private ModSortOrder order;
        public ModSorter_ByFile(ModSortOrder order)
        {
            this.order = order;
        }

        public IEnumerable<ModDetail> Sort(IEnumerable<ModDetail> mods)
        {
            return order switch
            {
                ModSortOrder.Ascending => mods.OrderBy(m => m.FileName),
                ModSortOrder.Descending => mods.OrderByDescending(m => m.FileName)
            };
        }
    }
}
