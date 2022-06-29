using L4d2_Mod_Manager.Domain;
using L4d2_Mod_Manager.Domain.Repository;
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
    public class ModOperation
    {
        private static ModFileRepository modFileRepo = new ModFileRepository();
        private const string TitleKeyName = "addontitle";
        private const string VersionKeyName = "addonversion";
        private const string TaglineKeyName = "addontagline";
        private const string AuthorKeyName = "addonauthor";

        public static IEnumerable<(int id, string img, string name, string vpkid, string path)> ModInfos()
        {
            return ModRepository.Instance.GetMods()
                .Select(m => 
                (m.Id,
                ModFP.SelectPreview(m),
                ModFP.SelectName(m)
                , m.vpkId
                ,ModFileService.FindFileById(m.FileId).Map(x => x.FilePath).ValueOr(""))
            );
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
            var kvs = PairQue(words).ToArray();

            var lookup = kvs.ToLookup(x => x.Item1.ToLower(), x => x.Item2);

            return new ModInfo(
                lookup.FindElementSafe(TitleKeyName),
                lookup.FindElementSafe(VersionKeyName),
                lookup.FindElementSafe(TaglineKeyName),
                lookup.FindElementSafe(AuthorKeyName)
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
                    WorkshopPreviewImage = info.PreviewImage
                });
            return newMod.Match(x => (x, true), () => (mod, false));
        }

        static StreamWriter fs = new StreamWriter(File.OpenWrite("out.txt"));
        private static string[] ParseLine(string line)
        {
            line = line.Trim();
            var matches = Regex.Matches(line, "\"(?<a>[^\"]+)\"|(?<a>[a-zA-Z0-9_]+)|//.*$");
            var words = matches.Select(m => m.Groups["a"].Value).ToArray();

            if (words.Length == 2)
            {
                fs.WriteLine($"{words[0]} -- {words[1]}");
                fs.Flush();
                return words;
            }
            else
            {
                return new string[0];
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
    }
}
