using Infrastructure.Utility;
using Steamworks;
using Steamworks.Ugc;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.WorkshopInfoModule.AddonInfoDownload
{
    class SteamworksAddonInfoDownloadStrategy : IAddonInfoDownloadStrategy
    {

        public static Maybe<SteamworksAddonInfoDownloadStrategy> CreateStrategy()
        {
            try
            {
                return Maybe.Some(new SteamworksAddonInfoDownloadStrategy());
            }
            catch 
            {
                return Maybe.None;
            }
        }

        public const int AppID = 550;

        public string StrategyName => "SteamWorks模式";

        private SteamworksAddonInfoDownloadStrategy()
        {
            SteamClient.Init(550);
        }

        ~SteamworksAddonInfoDownloadStrategy()
        {
            SteamClient.Shutdown();
        }


        public async Task<Maybe<ModWorkshopInfo>> DownloadAddonInfoAsync(ulong vpkid)
        {
            try
            {
                ResultPage? result = await Query.All.WithFileId(vpkid).GetPageAsync(1).ConfigureAwait(false);
                if (!result.HasValue || result.Value.ResultCount != 1)
                {
                    return Maybe.None;
                }

                Item item = result.Value.Entries.First();
                result.Value.Dispose();

                //var res = await SteamUGC.QueryFileAsync(new() { Value = vpkid });
                var f = DownloadImageFromURL(item.PreviewImageUrl);

                var owner = item.Owner;
                // 下载用户名称
                await owner.RequestInfoAsync();
                var author = item.Owner.Name;
                return Maybe.Some(new ModWorkshopInfo(author, item.Title, item.Description, f, item.Tags.ToImmutableArray()));
            }
            catch (Exception e)
            {
                return Maybe.None;
            }
        }

        /// <summary>
        /// 从URL中下载图片，返回图片的本地路径
        /// </summary>
        /// <returns>下载图片的本地路径</returns>
        private static string DownloadImageFromURL(string url)
        {
            string fileName = Path.GetTempFileName();
            try
            {
                HttpWebRequest previewImgReq = (HttpWebRequest)WebRequest.Create(url);
                var previewImgRes = previewImgReq.GetResponse();

                var imageType = GetImageTypeFromContentType(previewImgRes.ContentType);
                fileName = Path.ChangeExtension(fileName, "." + imageType);

                using var previewImgStream = previewImgRes.GetResponseStream();
                using FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
                byte[] buf = new byte[1024];
                while (true)
                {
                    var len = previewImgStream.Read(buf, 0, buf.Length);
                    if (len > 0)
                    {
                        fs.Write(buf, 0, len);
                    }
                    else
                    {
                        break;
                    }
                }
                return fileName;
            }
            catch
            {
                return "";
            }
        }


        private static string GetImageTypeFromContentType(string str)
        {
            if (str.StartsWith("image/"))
            {
                //截断 "image/" 字符串
                return str.Substring(6);
            }
            else
            {
                throw new Exception("Unsupported Content Type:" + str);
            }
        }
    }
}
