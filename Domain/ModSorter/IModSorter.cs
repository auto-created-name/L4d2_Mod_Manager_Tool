using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModSorter
{
    /// <summary>
    /// 模组排序顺序
    /// </summary>
    public enum ModSortOrder { 
        /// <summary>
        /// 升序
        /// </summary>
        Ascending,
        /// <summary>
        /// 降序
        /// </summary>
        Descending 
    }
    public interface IModSorter
    {
        IEnumerable<ModBrief> Sort(IEnumerable<ModBrief> mods);
    }
}
