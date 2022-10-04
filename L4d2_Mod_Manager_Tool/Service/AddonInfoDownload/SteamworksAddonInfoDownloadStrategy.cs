using Infrastructure.Utility;
using L4d2_Mod_Manager_Tool.Domain;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Service.AddonInfoDownload
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


        public Maybe<ModWorkshopInfo> DownloadAddonInfo(ulong vpkid)
        {
            try
            {
                var task = SteamUGC.QueryFileAsync(new() { Value = vpkid });
                // 等待5秒超时取消
                if (task.Wait(5000))
                {
                    var res = task.Result;

                    var f = DownloadImageFromURL(res.Value.PreviewImageUrl);
                    return Maybe.Some(new ModWorkshopInfo(res.Value.Title, res.Value.Description, f, res.Value.Tags.ToImmutableArray()));
                }
                else
                {
                    return Maybe.None;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
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
