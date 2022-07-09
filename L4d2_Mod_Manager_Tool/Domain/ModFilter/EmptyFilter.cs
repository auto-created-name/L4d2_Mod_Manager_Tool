using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.ModFilter
{
    class EmptyFilter : IModFilter
    {
        public IEnumerable<Mod> FilterMod(IEnumerable<Mod> mods) => mods;
    }
}
