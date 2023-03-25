using Steamworks;
using Steamworks.Ugc;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Core.SteamWorksModModule
{
    public class SteamWorksMod : IMod
    {
        public VpkId Id { get; private set; }
        public SteamWorksMod(VpkId id)
        {
            Id = id;
        }

        public async Task<string> RequestModName()
        {
           var item = await SteamUGC.QueryFileAsync((ulong)Id.Id);
            if (item.HasValue)
                return item.Value.Title;
            else
                return "";
        }

        public async Task<bool> Subscribe()
        {
            Item? item = await QueryItemWithFieldId((ulong)Id.Id);

            // 未找到模组
            if (!item.HasValue)
                return false;

            //FIXME:Item.IsSubscribed总是返回false，API已经过期，无法获取正确的结果！
            // 已经订阅的模组不能重复订阅
            if (item.Value.IsSubscribed)
                return false;

            return await item.Value.Subscribe().ConfigureAwait(false);
        }

        public async Task<bool> Unsubscribe()
        {
            Item? item = await QueryItemWithFieldId((ulong)Id.Id);

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
