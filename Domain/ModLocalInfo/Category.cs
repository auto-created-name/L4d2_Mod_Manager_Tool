using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModLocalInfo {
    public record Category(string Name)
    {
        public static Category[] ParseGroup(string str)
            => str.Split(',').Select(str => new Category(str)).ToArray();

        public static string Concat(Category[] cats)
            => string.Join(',', cats.Select(cat => cat.Name));
    }
}
