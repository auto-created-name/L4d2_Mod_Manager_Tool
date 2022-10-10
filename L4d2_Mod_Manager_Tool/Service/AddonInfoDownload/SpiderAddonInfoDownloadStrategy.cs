using HtmlAgilityPack;
using Infrastructure.Utility;
using L4d2_Mod_Manager_Tool.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Service.AddonInfoDownload
{
    class SpiderAddonInfoDownloadStrategy : IAddonInfoDownloadStrategy
    {
        public string StrategyName => "网络爬虫模式";

        public Task<Maybe<ModWorkshopInfo>> DownloadAddonInfoAsync(ulong itemId)
        {
            string url = $"https://steamcommunity.com/sharedfiles/filedetails/?id={itemId}";
            return Task.Run(() =>
            {
                try
                {
                    HtmlWeb webClient = new();
                    var doc = webClient.Load(url);

                    var author = GetWorkShopItemAuthor(doc);
                    var tags = GetWorkshopItemTags(doc);
                    var title = GetWorkshopItemTitle(doc);
                    var desc = GetWorkshopItemDescription(doc);
                    string previewImageUrl = GetWorkshopPreviewImageUrl(doc);

                    var imgFile = DownloadImageFromURL(previewImageUrl);
                    return Maybe.Some(new ModWorkshopInfo(author, title, desc, imgFile, tags.ToImmutableArray()));
                }
                catch
                {
                    return Maybe.None;
                }
            });
        }

        private static string GetWorkShopItemAuthor(HtmlDocument doc)
        {
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='friendBlockContent']");
            return string.Join(',', nodes.Select(x => 
            {
                var nameNode = x.ChildNodes[0];
                return Regex.Replace(nameNode.InnerText, @"\s", "");
            }));
        }

        /// <summary>
        /// 获取创意工坊物品标题
        /// </summary>
        private static string GetWorkshopItemTitle(HtmlDocument doc)
        {
            var node = doc.DocumentNode.SelectSingleNode("//div[@class='workshopItemTitle']");
            return GetInnerTextOrEmpty(node);
        }

        /// <summary>
        /// 获取创意工坊物品描述
        /// </summary>
        private static string GetWorkshopItemDescription(HtmlDocument doc)
        {
            var node = doc.DocumentNode.SelectSingleNode("//div[@class='workshopItemDescription']");
            return GetInnerTextOrEmpty(node);
        }

        /// <summary>
        /// 获取创意工坊物品预览图片的URL
        /// </summary>
        private static string GetWorkshopPreviewImageUrl(HtmlDocument doc)
        {
            var node = doc.DocumentNode.SelectSingleNode("//img[@class='workshopItemPreviewImageMain']");
            if (node == null)
                return "";
            else
                return node.GetAttributeValue("src", "");
        }

        private static IEnumerable<string> GetWorkshopItemTags(HtmlDocument doc)
        {
            var tagNode = doc.DocumentNode.SelectNodes("//div[@class='workshopTags']/a");
            if (tagNode == null)
                return Enumerable.Empty<string>();
            else
                return tagNode.Select(node => node.InnerText);
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

        private static string GetInnerTextOrEmpty(HtmlNode node)
            => node?.InnerText ?? string.Empty;
    }
}
