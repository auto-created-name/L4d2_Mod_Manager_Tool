using L4d2_Mod_Manager_Tool.Domain;
using L4d2_Mod_Manager_Tool.Domain.ModFilter;
using L4d2_Mod_Manager_Tool.Domain.ModSorter;
using L4d2_Mod_Manager_Tool.Domain.Repository;
using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Service
{

    public class ModOperation
    {
        private static ModFileRepository modFileRepo = new ModFileRepository();
        private static ModRepository Repo { get => ModRepository.Instance; }
        private const string TitleKeyName = "addontitle";
        private const string VersionKeyName = "addonversion";
        private const string TaglineKeyName = "addontagline";
        private const string AuthorKeyName = "addonauthor";
        private const string DescriptKeyName = "addondescription";

        private static ModFilterBuilder filterBuilder = new();

        /// <summary>
        /// 增加模组标签过滤
        /// </summary>
        public static void AddModFilterTag(string tagName)
            => filterBuilder.AddTag(tagName);

        /// <summary>
        /// 删除模组标签过滤
        /// </summary>
        public static void RemoveModFilterTag(string tagName)
            => filterBuilder.RemoveTag(tagName);

        /// <summary>
        /// 增加模组分类过滤
        /// </summary>
        public static void AddModFilterCategory(string catName)
            => filterBuilder.AddCategory(catName);

        /// <summary>
        /// 删除模组分类过滤
        /// </summary>
        public static void RemoveModFilterCategory(string catName)
            => filterBuilder.RemoveCategory(catName);

        public static void SetModFilterName(string name)
            => filterBuilder.SetName(name);

        public static Maybe<Mod> FindModById(int modId)
            => Repo.FindModById(modId);

        public static IEnumerable<ModDetail> FilteredModInfo()
        {
            return modSorter.Sort(
                filterBuilder.FinalFilter
                .FilterMod(ModRepository.Instance.GetMods())
                ).Select(GetModDetail);
        }
        #region 模组排序
        private static IModSorter modSorter = new ModSorter_Default();
        public static void SetModSortMod(string label)
        {
            modSorter = label switch
            {
                "名称" => new ModSorter_ByName(ModSortOrder.Ascending),
                _ => new ModSorter_Default()
            };
        }
        #endregion
        /// <summary>
        /// 通过文件名找到模组
        /// </summary>
        public static Maybe<Mod> FindModByFileName(string fn)
        {
            return ModFileService.FindFileByFileName(fn)
                .Bind(x => Repo.FindModByFileId(x.Id));
        }

        public static IEnumerable<ModDetail> ModInfos()
        {
            return ModRepository.Instance.GetMods()
                .Select(GetModDetail);
        }

        public static ModDetail GetModDetail(Mod m)
        {
            return new(m.Id,
                ModFP.SelectPreview(m)
                , ModFP.SelectName(m)
                , m.vpkId
                , m.Author
                , m.Tagline
                , ModFP.CategoriesSingleLine(m)
                , ModFP.TagsSingleLine(m)
                , ModFP.SelectDescription(m));
        }

        public static Maybe<ModDetail> GetModDetail(int modId)
        {
            return Repo.FindModById(modId).Map(GetModDetail);
        }

        public static IEnumerable<Mod> FilterMod(string filter)
        {
            return ModRepository.Instance.GetMods().Where(m => FilterMod(filter, m));
        }

        public static bool UpdateMod(Mod mod)
        {
            return ModRepository.Instance.UpdateMod(mod);
        }

        public static ModInfo ReadModInfo(string addoninfo)
        {
            //string content = File.ReadAllText(addoninfo);
            var lines = File.ReadLines(addoninfo);
            var words = lines.SelectMany(l => ParseLine(l)).ToArray();
            //content = PruningAddonInfo(content);
            //
            //var matches = Regex.Matches(content, "\"(?<val>[\\S ]+)\"|//.*|(?<val>\\S+)");
            ////var kvs = ParseKeyValuePairs(content.ToCharArray()).ToArray();
            //var words = matches
            //    .Where(x => !x.Value.StartsWith("//"))
            //    .Select(x => x.Groups["val"].Value)
            //    .ToArray();
            var kvs = PairQue(words).Select(x => (x.Item1.ToLower(), x.Item2)).ToArray();

            var lookup = kvs.ToLookup(x => x.Item1, x => x.Item2);

            // addoninfo 中的分类信息
            var categories = kvs
                .Where(x => KeyIsCategoryType(x.Item1))
                .Select(PairToCategory)
                .Select(x => x.ValueOr(null))
                .Where(x => x != null)
                .ToArray();

            return new ModInfo(
                lookup.FindElementSafe(TitleKeyName),
                lookup.FindElementSafe(VersionKeyName),
                lookup.FindElementSafe(TaglineKeyName),
                lookup.FindElementSafe(AuthorKeyName),
                lookup.FindElementSafe(DescriptKeyName),
                categories.ToImmutableArray()
            );
        }

        /// <summary>
        /// 将模组预览图存入本地缓存
        /// </summary>
        public static Mod MoveWorkshopPreviewImage(Mod mod)
        {
            if (string.IsNullOrEmpty(mod.WorkshopPreviewImage))
                return mod;
            if (!File.Exists(mod.WorkshopPreviewImage))
                return mod;

            var modFile = modFileRepo.FindModFileById(mod.FileId);
            var newMod = modFile.Map(mf =>
            {
                string fileName = Path.GetFileName(mf.FilePath);
                string tpath = VPKServices.GetVPKTemporayPath(fileName);
                string workshopPreviewImageLoc = Path.Combine(tpath, "workshopPreviewImage" + Path.GetExtension(mod.WorkshopPreviewImage));
                File.Move(mod.WorkshopPreviewImage, workshopPreviewImageLoc, true);
                return mod with { WorkshopPreviewImage = workshopPreviewImageLoc };
            });
            return newMod.ValueOr(mod);
        }

        private static bool FilterMod(string filter, Mod mod)
        {
            if (string.IsNullOrEmpty(filter))
                return true;
            else
                return
                    mod.WorkshopTitle.Contains(filter, StringComparison.CurrentCultureIgnoreCase)
                    || mod.Title.Contains(filter, StringComparison.CurrentCultureIgnoreCase)
                    || mod.Author.Contains(filter, StringComparison.CurrentCultureIgnoreCase)
                    || mod.vpkId.Contains(filter);
        }

        //static StreamWriter fs = new StreamWriter(File.OpenWrite("out.txt"));
        private static IEnumerable<string> ParseLine(string line)
        {
            line = line.Trim();
            var matches = Regex.Matches(line, "\"(?<a>[^\"]+)\"|(?<a>[a-zA-Z0-9_]+)|//.*$");
            var words = matches.Select(m => m.Groups["a"].Value).ToArray();

            if (words.Length >= 2)
            {
                //fs.WriteLine($"{words[0]} -- {words[1]}");
                //fs.Flush();
                return words.Take(2);
            }
            else
            {
                return Array.Empty<string>();
            }
        }

        private static IEnumerable<(T,T)> PairQue<T>(T[] xs)
        {
            if (xs.Length < 2) 
                yield break;

            for(int i = 1; i < xs.Length; i += 2)
                yield return (xs[i - 1], xs[i]);
        }

        /// <summary>
        /// 解析键值对，找到分类
        /// </summary>
        private static Maybe<string> PairToCategory((string, string) pair)
        {
            return pair.Item1 switch
            {
                "addoncontent_bossinfected"     => pair.Item2.Equals("1") ? "Special Infected"  : Maybe.None,
                "addoncontent_campaign"         => pair.Item2.Equals("1") ? "Campaign"          : Maybe.None,
                "addoncontent_commoninfected"   => pair.Item2.Equals("1") ? "Common Infected"   : Maybe.None,
                "addoncontent_map"              => pair.Item2.Equals("1") ? "Map"               : Maybe.None,
                "addoncontent_music"            => pair.Item2.Equals("1") ? "Music"             : Maybe.None,
                "addoncontent_prefab"           => pair.Item2.Equals("1") ? "Prefab"            : Maybe.None,
                "addoncontent_prop"             => pair.Item2.Equals("1") ? "Prop"              : Maybe.None,
                "addoncontent_realism"          => pair.Item2.Equals("1") ? "Realism"           : Maybe.None,
                "addoncontent_scavenge"         => pair.Item2.Equals("1") ? "Scavenge"          : Maybe.None,
                "addoncontent_script"           => pair.Item2.Equals("1") ? "Script"            : Maybe.None,
                "addoncontent_skin"             => pair.Item2.Equals("1") ? "Skin"              : Maybe.None,
                "addoncontent_sound"            => pair.Item2.Equals("1") ? "Sound"             : Maybe.None,
                "addoncontent_spray"            => pair.Item2.Equals("1") ? "Spray"             : Maybe.None,
                "addoncontent_survival"         => pair.Item2.Equals("1") ? "Survival"          : Maybe.None,
                "addoncontent_survivor"         => pair.Item2.Equals("1") ? "Survivor"          : Maybe.None,
                "addoncontent_versus"           => pair.Item2.Equals("1") ? "Versus"            : Maybe.None,
                "addoncontent_weapon"           => pair.Item2.Equals("1") ? "Weapon"            : Maybe.None,
                "addoncontent_weaponmodel"      => pair.Item2.Equals("1") ? "WeaponModel"       : Maybe.None,
                _                               => Maybe.None
            };
        }

        /// <summary>
        /// 模组信息键为分类类型
        /// </summary>
        private static bool KeyIsCategoryType(string key)
        {
            return key.StartsWith("addoncontent_");
        }
    }
}
