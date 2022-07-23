using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.ModSorter
{
    class ModSorter_Default : IModSorter
    {
        public IEnumerable<Mod> Sort(IEnumerable<Mod> mods) => mods;
    }
}
