using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.SteamWorksModModule
{
    /// <summary>
    /// 构想不同的连接方式（SteamWorks，爬虫模式），将生成不同的模组实例。虽然它们会使用同样的数据，但是有些行为将无法扩展（例如订阅和退订模组）。
    /// 因此尝试抽象出它们的共同接口
    /// </summary>
    public interface IMod
    {
        Task<bool> Subscribe();

        Task<bool> Unsubscribe();
    }
}
