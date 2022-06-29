using L4d2_Mod_Manager.Domain;
using L4d2_Mod_Manager.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager.Service
{
    public class Spider
    {
        public static Maybe<ModWorkshopInfo> CollectModInfo(string vpkNumber)
        {
            if (!IsVpkNumber(vpkNumber)) return Maybe.None;

            string url = $"https://steamcommunity.com/sharedfiles/filedetails/?id={vpkNumber}";
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                string certFile = @"C:\Users\Louis\Desktop\steamworkshop.cer";
                X509Certificate2 certificate = new X509Certificate2(certFile);

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Timeout = 5000;
                req.ClientCertificates.Add(certificate);
                req.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);

                var res = req.GetResponse();
                using var reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                string html = reader.ReadToEnd();
                //string html = File.ReadAllText("web.txt");

                string title = GetWorkshopItemTitle(html);
                string descrip = GetWorkshopItemDescription(html);
                string previewImageUrl = GetWorkshopPreviewImageUrl(html);

                var imgFile = DownloadImageFromURL(previewImageUrl);
                return Maybe.Some(new ModWorkshopInfo(title, descrip, imgFile));
            }
            catch
            {
                return Maybe.None;
            }
        }

        /// <summary>
        /// 字符串为VPK串格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsVpkNumber(string str)
        {
            return Regex.IsMatch(str, @"^[1-9]?[0-9]{9}$");
        }

        //public static Maybe<int> ToVpkNumber(string numberStr)
        //{
        //    if (IsVpkNumber(numberStr))
        //    {
        //        return int.Parse(numberStr);
        //    }
        //    else
        //    {
        //        return Maybe.None;
        //    }
        //}

        /// <summary>
        /// 从html中获取创意工坊物品标题
        /// </summary>
        private static string GetWorkshopItemTitle(string html)
        {
            var match = Regex.Match(html, @"<div class=""workshopItemTitle"">\s*([\S ]+)\s*</div>");
            return match.Groups[1].Value;
        }

        /// <summary>
        /// 从html中获取创意工坊物品描述
        /// </summary>
        private static string GetWorkshopItemDescription(string html)
        {
            var match = Regex.Match(html, @"<div class=""workshopItemDescription"" id=""highlightContent"">([\S ]+)</div>");
            return match.Groups[1].Value;
        }

        /// <summary>
        /// 从html中获取创意工坊物品预览图片的URL
        /// </summary>
        private static string GetWorkshopPreviewImageUrl(string html)
        {
            var match = Regex.Match(html, @"<img id=""previewImageMain"" class=""workshopItemPreviewImageMain"" src=""([\S ]+)""/>");
            return match.Groups[1].Value;
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

        private static bool CheckValidationResult(object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors errors)
        {
            //直接确认
            return true;
        }
    }
}
