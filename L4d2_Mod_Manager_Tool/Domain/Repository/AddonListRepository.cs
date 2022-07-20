using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L4d2_Mod_Manager_Tool.Utility;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    class AddonListRepository
    {
        private Dictionary<int, bool> enabledDic = new();
        private Dictionary<string, bool> missingEnabledDic = new();

        /// <summary>
        /// 模组启用列表
        /// </summary>
        public IEnumerable<(int, bool)> AddonList 
            => enabledDic.Select(p => (p.Key, p.Value));

        /// <summary>
        /// 未识别的模组启用列表
        /// </summary>
        public IEnumerable<(string, bool)> MissingAddonList 
            => missingEnabledDic.Select(p => (p.Key, p.Value));
        public void AddRange(IEnumerable<(int, bool)> datas)
        {
            datas.Iter(t => enabledDic.Add(t.Item1, t.Item2));
        }

        public void AddMissingAddonInfos(IEnumerable<(string, bool)> datas)
        {
            datas.Iter(t => missingEnabledDic.Add(t.Item1, t.Item2));
        }

        public void Clear()
        {
            enabledDic.Clear();
            missingEnabledDic.Clear();
        }

        public void SetModEnabled(int modId, bool enabled)
        {
            enabledDic[modId] = enabled;
        }

        public Maybe<bool> ModEnabled(int modId)
        {
            if (enabledDic.TryGetValue(modId, out bool b))
                return Maybe.Some(b);
            else
                return Maybe.None;
        }
    }
}
