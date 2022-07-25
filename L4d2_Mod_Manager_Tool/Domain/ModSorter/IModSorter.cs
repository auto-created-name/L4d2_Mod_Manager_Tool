using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.ModSorter
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
    interface IModSorter
    {
        IEnumerable<ModDetail> Sort(IEnumerable<ModDetail> mods);
    }
}
