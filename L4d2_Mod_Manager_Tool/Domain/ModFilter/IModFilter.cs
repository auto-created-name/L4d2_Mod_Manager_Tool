using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.ModFilter
{
    /// <summary>
    /// 模组过滤器接口
    /// </summary>
    public interface IModFilter
    {
        /// <summary>
        /// 过滤模组
        /// </summary>
        IEnumerable<Mod> FilterMod(IEnumerable<Mod> mods);
    }

    public static class ModFilterFP
    {
        public static IModFilter And(this IModFilter filter, IModFilter otherFilter)
        {
            return new AndFilter(filter, otherFilter);
        }

        public static IModFilter Or(this IModFilter filter, IModFilter otherFilter)
        {
            return new OrFilter(filter, otherFilter);
        }
    }
}
