using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public record ModDetail(
        int Id,
        VpkId VpkId,
        string Name,
        string FileName,
        bool Enabled,
        string Author,
        string Tagline,
        string Tags,
        string Categories
    )
    {
        public static ModDetail Default
            => new(0, VpkId.Undefined, string.Empty, string.Empty, false, string.Empty, string.Empty, string.Empty, string.Empty);
    }
}
