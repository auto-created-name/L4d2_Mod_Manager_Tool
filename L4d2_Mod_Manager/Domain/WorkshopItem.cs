using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager.Domain
{
    /// <summary>
    /// 创意工坊项，在steam创意工坊中下载的在线信息
    /// </summary>
    public record WorkshopItem(string Id, string Title, string Descript, string Preview);
}
