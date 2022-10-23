using Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.ModLocalInfo
{
    internal class AddonInfo
    {
        private const string TitleKeyName = "addontitle";
        private const string VersionKeyName = "addonversion";
        private const string TaglineKeyName = "addontagline";
        private const string AuthorKeyName = "addonauthor";
        private const string DescriptKeyName = "addondescription";

        public AddonInfo(string content) 
        {
            var lines = Lines(content);
            var words = lines.SelectMany(l => ParseLine(l)).ToArray();
            var kvs = PairQue(words).Select(x => (x.Item1.ToLower(), x.Item2)).ToArray();

            var lookup = kvs.ToLookup(x => x.Item1, x => x.Item2);

            // addoninfo 中的分类信息
            var categories = kvs
                .Where(x => KeyIsCategoryType(x.Item1))
                .Select(PairToCategory)
                .Where(x => x != null)
                .ToArray();

            Title = lookup.FindElementSafe(TitleKeyName).ValueOr("");
            Version = lookup.FindElementSafe(VersionKeyName).ValueOr("");
            Tagline = lookup.FindElementSafe(TaglineKeyName).ValueOr("");
            Author = lookup.FindElementSafe(AuthorKeyName).ValueOr("");
            Description = lookup.FindElementSafe(DescriptKeyName).ValueOr("");
            Categories = categories.Select(s => new Category(s)).ToArray();
        }

        public string Title { get; private set; }
        public string Version { get; private set; }
        public string Tagline { get; private set; }
        public string Author { get; private set; }
        public string Description { get; private set; }
        public Category[] Categories { get; private set; }

        public static AddonInfo FromData(byte[] data)
        {
            using (var reader = new StreamReader(new MemoryStream(data)))
                return new AddonInfo(reader.ReadToEnd());
        }

        /// <summary>
        /// 解析键值对，找到分类
        /// </summary>
        private static string PairToCategory((string, string) pair)
        {
            return pair.Item1 switch
            {
                "addoncontent_bossinfected" => pair.Item2.Equals("1") ? "Special Infected" : null,
                "addoncontent_campaign" => pair.Item2.Equals("1") ? "Campaign" : null,
                "addoncontent_commoninfected" => pair.Item2.Equals("1") ? "Common Infected" : null,
                "addoncontent_map" => pair.Item2.Equals("1") ? "Map" : null,
                "addoncontent_music" => pair.Item2.Equals("1") ? "Music" : null,
                "addoncontent_prefab" => pair.Item2.Equals("1") ? "Prefab" : null,
                "addoncontent_prop" => pair.Item2.Equals("1") ? "Prop" : null,
                "addoncontent_realism" => pair.Item2.Equals("1") ? "Realism" : null,
                "addoncontent_scavenge" => pair.Item2.Equals("1") ? "Scavenge" : null,
                "addoncontent_script" => pair.Item2.Equals("1") ? "Script" : null,
                "addoncontent_skin" => pair.Item2.Equals("1") ? "Skin" : null,
                "addoncontent_sound" => pair.Item2.Equals("1") ? "Sound" : null,
                "addoncontent_spray" => pair.Item2.Equals("1") ? "Spray" : null,
                "addoncontent_survival" => pair.Item2.Equals("1") ? "Survival" : null,
                "addoncontent_survivor" => pair.Item2.Equals("1") ? "Survivor" : null,
                "addoncontent_versus" => pair.Item2.Equals("1") ? "Versus" : null,
                "addoncontent_weapon" => pair.Item2.Equals("1") ? "Weapon" : null,
                "addoncontent_weaponmodel" => pair.Item2.Equals("1") ? "WeaponModel" : null,
                _ => null
            };
        }

        /// <summary>
        /// 模组信息键为分类类型
        /// </summary>
        private static bool KeyIsCategoryType(string key)
        {
            return key.StartsWith("addoncontent_");
        }

        private static IEnumerable<(T, T)> PairQue<T>(T[] xs)
        {
            if (xs.Length < 2)
                yield break;

            for (int i = 1; i < xs.Length; i += 2)
                yield return (xs[i - 1], xs[i]);
        }

        private static IEnumerable<string> ParseLine(string line)
        {
            line = line.Trim();
            var matches = Regex.Matches(line, "\"(?<a>[^\"]+)\"|(?<a>[a-zA-Z0-9_]+)|//.*$");
            var words = matches.Select(m => m.Groups["a"].Value).ToArray();

            if (words.Length >= 2)
            {
                return words.Take(2);
            }
            else
            {
                return Array.Empty<string>();
            }
        }

        private static IEnumerable<string> Lines(string str)
        {
            using var reader = new StringReader(str);
            string line;
            while((line = reader.ReadLine()) != null)
                yield return line;
        }
    }
}
