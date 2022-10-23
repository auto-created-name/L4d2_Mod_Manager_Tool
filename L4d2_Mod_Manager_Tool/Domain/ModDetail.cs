using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain
{
    public record ModDetail(
        int Id,
        string Name,
        string FileName,
        bool Enabled,
        string Author,
        string Tagline
    )
    {
        public static ModDetail Default
            => new(0, string.Empty, string.Empty, false, string.Empty, string.Empty);
    }
}
