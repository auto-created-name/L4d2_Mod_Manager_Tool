using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.WorkshopInfoModule
{
    public record Tag(string Name)
    {
        public static Tag[] ParseGroup(string str)
            => str.Split(',').Select(s => new Tag(s)).ToArray();

        public static string Concat(Tag[] tags)
            => string.Join(',', tags.Select(t => t.Name));
    };
}
