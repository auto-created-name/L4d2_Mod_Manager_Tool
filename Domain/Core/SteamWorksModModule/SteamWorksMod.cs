using Steamworks.Ugc;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Core.SteamWorksModModule
{
    public class SteamWorksMod : IMod
    {
        private VpkId _id;
        public SteamWorksMod(VpkId id)
        {
            _id = id;
        }

        public async Task<bool> Subscribe()
        {
            Item? item = await QueryItemWithFieldId((ulong)_id.Id);

            // 未找到模组
            if (!item.HasValue)
                return false;

            // 已经订阅的模组不能重复订阅
            if (item.Value.IsSubscribed)
                return false;

            return await item.Value.Subscribe().ConfigureAwait(false);
        }

        public async Task<bool> Unsubscribe()
        {
            Item? item = await QueryItemWithFieldId((ulong)_id.Id);

            // 未找到模组
            if (!item.HasValue)
                return false;

            // 未订阅的模组不退订
            if (!item.Value.IsSubscribed)
                return false;

            return await item.Value.Unsubscribe().ConfigureAwait(false);
        }

        private static async Task<Item?> QueryItemWithFieldId(ulong id)
        {
            ResultPage? result = await Query.All.WithFileId(id).GetPageAsync(1).ConfigureAwait(false);
            if (!result.HasValue || result.Value.ResultCount != 1)
            {
                return null;
            }

            Item item = result.Value.Entries.First();
            result.Value.Dispose();
            return item;
        }
    }
}
