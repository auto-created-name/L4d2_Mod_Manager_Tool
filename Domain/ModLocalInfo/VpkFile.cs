using Domain.Core;
using Infrastructure.Utility;
using SharpVPK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModLocalInfo
{
    internal class VpkFile
    {
        private VpkArchive archive;
        private VpkEntry[] entries;
        private string fileName;

        public VpkFile(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException(file);

            fileName = file;
            archive = new();
            archive.Load(file);
            entries = archive.Directories.SelectMany(dir => dir.Entries).ToArray();
        }

        public Maybe<ImageFile> Image
        {
            get
            {
                // %USERNAME\AppData\Local\Temp\[VPK File]\
                var path = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(fileName));
                return entries.Where(IsAddonImage).FirstElementSafe().Map(entry => SaveEntry(entry, path)).Map(fn => new ImageFile(fn));
            }
        }

        public Maybe<AddonInfo> Info
                => entries.Where(IsAddonInfo).FirstElementSafe().Map(entry => AddonInfo.FromData(entry.Data));

        public Category[] Categories
        {
            get
            {
                var rulegroup = new CategoryRuleGroupFactory().Create();
                return entries.Select(FullName)
                    .SelectMany(rulegroup.MatchCategories)
                    .Distinct().Select(s => new Category(s)).ToArray();
            }
        }

        /// <summary>
        /// 将VpkEntry保存为文件
        /// </summary>
        private static string SaveEntry(VpkEntry entry, string path = "")
        {
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fn = Path.Combine(path, $"{entry.Filename}.{entry.Extension}");
            //string fn = Path.Combine(Path.GetTempPath(), fileName);
            File.WriteAllBytes(fn, entry.Data);
            return fn;
        }

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

        private static string FullName(VpkEntry entry)
        {
            return $"{entry.Path}/{entry.Filename}.{entry.Extension}";
        }
    }
}
