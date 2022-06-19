using L4d2_Mod_Manager.Arthitecture;
using L4d2_Mod_Manager.Domain;
using L4d2_Mod_Manager.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager.Service
{
    public class VPKServices
    {
        private const string VpkFolder = @"E:\Steam\steamapps\common\Left 4 Dead 2\left4dead2\addons";
        public static VpkSnippet[] ScanVPK()
        {
            var vpks = ListVpk(VpkFolder);
            var snippets = vpks.Select(vpk => SnippedVpk(vpk)).ToArray();
            return snippets;
        }

        /// <summary>
        /// 解压vpk并生成摘要
        /// </summary>
        private static VpkSnippet SnippedVpk(string vpk)
        {
            // 列出所有vpk内容，找到addonimage
            var files = VPKProcess.ListFile(vpk);
            Maybe<string> addonImageFile = FindAddonImage(files);

            string folder = GetVPKTemporayPath(vpk);
            //IEnumerable<string> extra = new string[] { "addoninfo.txt" };
            //extra = addonImageFile.Match(x => extra.Append(x), () => extra);

            return WithFolder(folder, dir => {
                var addoninfo = FindAddonInfo(files);
                var addoninfoFile = addoninfo.Bind(x =>
                {
                    try
                    {
                        VPKProcess.ExtractFile(vpk, dir, x);
                        return Maybe.Some(Path.Combine(dir, x));
                    }
                    catch
                    {
                        return Maybe.None;
                    }
                });

                var addonimage = FindAddonImage(files);
                var addonimageFile = addonimage.Bind(x =>
                {
                    try
                    {
                        VPKProcess.ExtractFile(vpk, dir, x);
                        if (x.EndsWith(".vtf"))
                        {
                            NoVtfConverter.ConverVtf(x);
                            x = Path.ChangeExtension(x, ".jpg");
                        }
                        return Maybe.Some(Path.Combine(dir, x));
                    }
                    catch
                    {
                        return Maybe.None;
                    }

                });
                //string[] extraArr = extra.ToArray();
                //VPKProcess.ExtractFile(vpk, folder, extraArr);

                // 如果有缩略图且图片是以vtf格式的，需要转码为jpg
                //if(extraArr.Length == 2 && extraArr[1].EndsWith(".vtf"))
                //{
                //    NoVtfConverter.ConverVtf(extraArr[1]);
                //}

                return new VpkSnippet(
                    Path.GetFileName(vpk),
                    addonimageFile,
                    addoninfoFile
                );
            });
        }

        /// <summary>
        /// 列出指定文件夹中所有vpk文件
        /// </summary>
        private static string[] ListVpk(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            if (di.Exists)
            {
                return di.GetFiles("*.vpk").Select(f => f.FullName).ToArray();
            }
            else
            {
                return new string[0];
            }
        }

        /// <summary>
        /// 从vpk文件生成临时文件
        /// </summary>
        public static string GetVPKTemporayPath(string vpk)
        {
            return Path.Combine(Path.GetTempPath(), "L4dModManagerTool", Path.GetFileNameWithoutExtension(vpk));
        }

        private static T WithFolder<T>(string folder, Func<string, T> f)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (Directory.Exists(folder))
            {
                return f(folder);
            }
            else
            {
                return default(T);
            }

        }
        #region 查找文件
        /// <summary>
        /// 找到addonimage文件
        /// </summary>
        private static Maybe<string> FindAddonImage(IEnumerable<string> files)
        {
            return FindAddonFileWith(str => Regex.IsMatch(str, @"^addonimage\..*$"), files);
        }

        /// <summary>
        /// 找到addoninfo文件
        /// </summary>
        private static Maybe<string> FindAddonInfo(IEnumerable<string> files)
        {
            return FindAddonFileWith(str => str.Equals("addoninfo.txt"), files);
        }

        /// <summary>
        /// 使用特定谓词查找文件
        /// </summary>
        private static Maybe<string> FindAddonFileWith(Func<string, bool> f, IEnumerable<string> files)
        {
            var res = files.Where(str => f(str)).ToArray();
            if (res.Length == 0)
                return Maybe.None;
            else
                return Maybe.Some(res[0]);
        }
        #endregion
    }
}
