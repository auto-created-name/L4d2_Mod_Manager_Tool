using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModFile
{
    public record ModFileChanged(ModFile[] Lost, ModFile[] New);
}
