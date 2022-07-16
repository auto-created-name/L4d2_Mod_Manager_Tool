using L4d2_Mod_Manager_Tool.Domain;
using L4d2_Mod_Manager_Tool.Domain.Repository;
using L4d2_Mod_Manager_Tool.Module.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Service
{
    static class AddonListService
    {

        public static void ConvertToDomain()
        {
            var als = ParseAddonList(ReadAddonListContent());
            var ds = als.Select(t => (ModOperation.FindModByFileName(t.Item1)
                                         .Map(m => m.Id).ValueOr(-1), t.Item2)).ToArray();
        }

        private static string AddonListFilePath =>
            Path.Combine(L4d2Folder.CoreFolder, "addonlist.txt");

        /// <summary>
        /// 读取模组文件列表内容
        /// </summary>
        private static string ReadAddonListContent()
        {
            string content = "";
            var mf = AddonListFilePath;
            if (!File.Exists(AddonListFilePath))
                return content;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var sr = new StreamReader(mf, Encoding.GetEncoding("GB2312"), true))
                content = sr.ReadToEnd();
            return content;
        }

        /// <summary>
        /// 从addonlist中解析模组信息
        /// </summary>
        private static List<(string,bool)> ParseAddonList(string content)
        {
            List<(string, bool)> addonLists = new();
            string contentLex = @"\{([.\S\s]+)\}";
            var contentMatch = Regex.Match(content, contentLex);
            var lex = @"""(.+?)""";
            var matches = Regex.Matches(content, lex);

            var words = matches.Select(match => match.Groups[1].Value).ToArray();
            if (words.Length == 0)
                return addonLists;

            if (!words[0].Equals("AddonList"))
                return addonLists;

            for (int i = 2; i < words.Length; i += 2)
            {
                addonLists.Add(new(words[i - 1], int.Parse(words[i]) == 1));
            }
            return addonLists;
        }
    }
}
