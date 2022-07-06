using L4d2_Mod_Manager_Tool.Domain;
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
    public record ModDetail(
        int Id, string Img, string Name, 
        string Vpkid, string Author, 
        string Tagline, string Categories, 
        string Tags, string Descript);
    public class ModOperation
    {
        private static ModFileRepository modFileRepo = new ModFileRepository();
        private static ModRepository Repo { get => ModRepository.Instance; }
        private const string TitleKeyName = "addontitle";
        private const string VersionKeyName = "addonversion";
        private const string TaglineKeyName = "addontagline";
        private const string AuthorKeyName = "addonauthor";

        public static IEnumerable<ModDetail> ModInfos()
        {
            return ModRepository.Instance.GetMods()
                .Select(GetModDetail);
        }

        /// <summary>
        /// 打开资源管理器，选中模组文件
        /// </summary>
        public static void ShowModInFileExplorer(int modId)
        {
            ModRepository.Instance.FindModById(modId)
                .Map(m => m.FileId)
                .Bind(ModFileService.FindFileById)
                .Map(f => Module.FileExplorer.FileExplorerUtils.OpenFileExplorerAndSelectItem(f.FilePath));
        }

        /// <summary>
        /// 使用资源管理器打开模组文件
        /// </summary>
        public static void OpenModFileInExplorer(int modId)
        {
            ModRepository.Instance.FindModById(modId)
                .Map(m => m.FileId)
                .Bind(ModFileService.FindFileById)
                .Map(f => Module.FileExplorer.FileExplorerUtils.OpenFileInExplorer(f.FilePath));
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
                , m.WorkshopDescript);
        }

        public static Maybe<ModDetail> GetModDetail(int modId)
        {
            return Repo.FindModById(modId).Map(GetModDetail);
        }

        public static IEnumerable<Mod> FilterMod(string filter)
        {
            return ModRepository.Instance.GetMods().Where(m => FilterMod(filter, m));
        }

        /// <summary>
        /// 取消激活模组
        /// </summary>
        public static void DeactiveMod(Mod mod)
        {
            var modFile = modFileRepo.FindModFileById(mod.FileId);
            modFile.Map(mf => ModFileService.DeactiveModFile(mf));
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

        /// <summary>
        /// 更新创意工坊数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public static (Mod, bool) UpdateWorkshopInfo(Mod mod)
        {
            var modFile = modFileRepo.FindModFileById(mod.FileId);
            var newMod = modFile
                .Bind(mf => Spider.CollectModInfo(ModFileFP.ModFileName(mf)))
                .Map(info => mod with
                {
                    WorkshopTitle = info.Title,
                    WorkshopDescript = info.Descript,
                    WorkshopPreviewImage = info.PreviewImage,
                    Tags = info.Tags
                });
            return newMod.Match(x => (x, true), () => (mod, false));
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

        static StreamWriter fs = new StreamWriter(File.OpenWrite("out.txt"));
        private static IEnumerable<string> ParseLine(string line)
        {
            line = line.Trim();
            var matches = Regex.Matches(line, "\"(?<a>[^\"]+)\"|(?<a>[a-zA-Z0-9_]+)|//.*$");
            var words = matches.Select(m => m.Groups["a"].Value).ToArray();

            if (words.Length >= 2)
            {
                fs.WriteLine($"{words[0]} -- {words[1]}");
                fs.Flush();
                return words.Take(2);
            }
            else
            {
                return Array.Empty<string>();
            }
        }

        /// <summary>
        /// 对原始addonInfo数据进行剪枝，方便后续处理
        /// </summary>
        private static string PruningAddonInfo(string content)
        {
            int lbrace = content.IndexOf('{');
            if(lbrace > 0)
                content = content.Substring(lbrace + 1);
            int rbrace = content.LastIndexOf('}');
            if(rbrace > 0)
                content = content.Substring(0, rbrace);

            //这里不能将//.*作为注释消除，因为包含网址的时候,http://...会被当成注释
            //content = Regex.Replace(content, @"//.*", "");
            //没有区别
            //content = Regex.Replace(content, @"\s+", " ");

            content = content.Trim();
            return content;
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
                "addoncontent_campaign" => pair.Item2.Equals("1") ? "Campaign" : Maybe.None,
                "addoncontent_map"      => pair.Item2.Equals("1") ? "Map"      : Maybe.None,
                "addoncontent_skin"     => pair.Item2.Equals("1") ? "Skin"     : Maybe.None,
                "addoncontent_weapon"   => pair.Item2.Equals("1") ? "Weapon"   : Maybe.None,
                "addoncontent_survivor" => pair.Item2.Equals("1") ? "Survivor" : Maybe.None,
                "addoncontent_sound"    => pair.Item2.Equals("1") ? "Sound"    : Maybe.None,
                "addoncontent_script"   => pair.Item2.Equals("1") ? "Script"   : Maybe.None,
                "addoncontent_prop"     => pair.Item2.Equals("1") ? "Prop"     : Maybe.None,
                _ => Maybe.None
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
