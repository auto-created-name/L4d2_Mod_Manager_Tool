using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModSorter
{
    public class ModSorter_Default : IModSorter
    {
        public IEnumerable<ModBrief> Sort(IEnumerable<ModBrief> mods) => mods;
    }
}
