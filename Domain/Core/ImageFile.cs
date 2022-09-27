using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public record ImageFile(string File)
    {
        public static ImageFile MissingImage
            => new("");
    }
}
