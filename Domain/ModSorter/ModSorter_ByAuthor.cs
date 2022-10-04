using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModSorter
{
    public class ModSorter_ByAuthor : IModSorter
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
