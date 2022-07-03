using L4d2_Mod_Manager_Tool.Arthitecture;
using L4d2_Mod_Manager_Tool.Domain;
using L4d2_Mod_Manager_Tool.Module.Settings;
using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SharpVPK;

namespace L4d2_Mod_Manager_Tool.Service
{
    public class VPKServices
    {
        /// <summary>
        /// 扫描所有的模组文件
        /// </summary>
        public static IEnumerable<ModFile> ScanAllModFile()
        {
            return ModFolders
                .SelectMany(ListVpk)
                .Select(file => ModFileFP.CreateModFile(file));
        }

        /// <summary>
        /// 将mod文件解压并生成mod信息
        /// </summary>
        /// <param name="mf"></param>
        /// <returns></returns>
        public static Mod ExtraMod(ModFile mf)
        {
            var snippet = SnippedVpk(mf.FilePath);
            var mod = ModFP.CreateMod(mf);
            // 赋值缩略图信息
            mod = snippet.AddonImage.Match(img => mod with { Thumbnail = img }, () => mod);
            mod = snippet.AddonInfo.Match(info =>
            {
                var modinfo = ModOperation.ReadModInfo(info);
                return mod with
                {
                    Title = modinfo.Title.ValueOr(""),
                    Version = modinfo.Version.ValueOr(""),
                    Tagline = modinfo.Tagline.ValueOr(""),
                    Author = modinfo.Author.ValueOr("")
                };
            }, () => mod);
            return mod;
        }

        //public static VpkSnippet[] ScanVPK()
        //{
        //    var vpks = ListVpk(VpkFolder);
        //    var snippets = vpks.Select(vpk => SnippedVpk(vpk)).ToArray().ToArray();
        //    return snippets;
        //}

        /// <summary>
        /// 解压vpk并生成摘要
        /// </summary>
        public static VpkSnippet SnippedVpk(string vpk)
        {
            VpkArchive archive = new();
            archive.Load(vpk);
            
            // 列出所有vpk内容，找到addonimage
            var files = archive.Directories.SelectMany(dir => dir.Entries).ToArray();

            var imgEntry = files.Where(IsAddonImage).FirstElementSafe();
            var infoEntry = files.Where(IsAddonInfo).FirstElementSafe();

            string folder = GetVPKTemporayPath(vpk);
            return WithFolder(folder, dir => {
                var addoninfoFile = infoEntry.Map(entry => {
                    string file = Path.Combine(folder, entry.Filename) + "." + entry.Extension;
                    ExtraVpkEntry(file, entry);
                    return file;
                });


                var addonimageFile = imgEntry.Map(entry => {
                    string file = Path.Combine(folder, entry.Filename) + "." + entry.Extension;
                    ExtraVpkEntry(file, entry);
                    if (file.EndsWith(".vtf"))
                    {
                        NoVtfConverter.ConverVtf(file);
                        file = Path.ChangeExtension(file, ".jpg");
                    }
                    return file;
                });

                //if (x.EndsWith(".vtf"))
                //{
                //    NoVtfConverter.ConverVtf(x);
                //    x = Path.ChangeExtension(x, ".jpg");
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
        public static IEnumerable<string> ListVpk(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            if (di.Exists)
            {
                return di.GetFiles("*.vpk").Select(f => f.FullName);
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

        /// <summary>
        /// 模组文件夹
        /// </summary>
        private static IEnumerable<string> ModFolders => SettingFP.GetSetting().modFileFolder;

        private static void ExtraVpkEntry(string file, VpkEntry entry)
        {
            // 卫语句
            if (string.IsNullOrEmpty(file))
                return;

            if (entry == null)
                return;
            var data = entry.Data;
            File.WriteAllBytes(file, data);
        }
        #region 查找文件
        /// <summary>
        /// 该文件是addonimage
        /// </summary>
        private static bool IsAddonImage(VpkEntry entry)
        {
            return entry.Path.Equals(" ") && entry.Filename.ToLower().Equals("addonimage"); 
        }

        /// <summary>
        /// 该文件是addoninfo
        /// </summary>
        private static bool IsAddonInfo(VpkEntry entry)
        {
            return entry.Path.Equals(" ") && entry.Filename.ToLower().Equals("addoninfo");
        }
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
