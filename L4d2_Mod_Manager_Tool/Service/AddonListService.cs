using L4d2_Mod_Manager_Tool.Domain.Repository;
using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace L4d2_Mod_Manager_Tool.Service
{
    static class AddonListService
    {
        public static AddonListRepository Repo { get; } = new AddonListRepository();

        public static void Load()
        {
            var als = ParseAddonList(ReadAddonListContent());
            var ds = als.Select(t => (
                ModOperation.FindModByFileName(t.Item1).Map(m => m.Id).ValueOr(-1), 
                t.Item1, 
                t.Item2)
                );
            var findedDomainModel = ds.Where(m => m.Item1 != -1);
            var missingDomainModel = ds.Where(m => m.Item1 == -1);
            Repo.Clear();
            Repo.AddRange(findedDomainModel.Select(t => (t.Item1, t.Item3)));
            Repo.AddMissingAddonInfos(missingDomainModel.Select(t => (t.Item2, t.Item3)));
        }

        public static bool IsModEnabled(int modId)
            => Repo.ModEnabled(modId).ValueOr(false);

        public static void SetModEnabled(int modId, bool enabled)
            => Repo.SetModEnabled(modId, enabled);

        private static string AddonListFilePath 
            => Path.Combine(L4d2Folder.CoreFolder, "addonlist.txt");

        public static void ApplyAddonList()
        {
            StringBuilder stringBuilder = new ();
            stringBuilder.AppendLine("\"AddonList\"").AppendLine("{");
            Repo.AddonList.Iter(t => 
            {
                var modFile = ModCrossServer.GetModFileByModId(t.Item1).ValueOrThrow("指定模组信息不存在");
                var enableStr = t.Item2 ? "1" : "0";
                stringBuilder.AppendLine($"\t\"{modFile}\"\t\t\"{enableStr}\"");
            });
            Repo.MissingAddonList.Iter(t =>
            {
                var enableStr = t.Item2 ? "1" : "0";
                stringBuilder.AppendLine($"\t\"{t.Item1}\"\t\t\"{enableStr}\"");
            });
            stringBuilder.AppendLine("}");

            WriteAddonListContent(stringBuilder.ToString());
        }

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

        private static void WriteAddonListContent(string content)
        {
            var mf = AddonListFilePath;
            string tmpfile = Path.GetTempFileName();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {
                var writer = new StreamWriter(tmpfile, false, Encoding.GetEncoding("GB2312"));
                writer.Write(content);
                writer.Dispose();
                // 备份，然后覆盖
                BackupAddonList();
                File.Move(tmpfile, mf, true);
            }catch(Exception e)
            {
                WinformUtility.ErrorMessageBox($"写入{mf}失败：{e.Message}", "错误");
            }
        }

        /// <summary>
        /// 备份目前的清单
        /// </summary>
        private static void BackupAddonList()
        {
            var mf = AddonListFilePath;
            File.Copy(mf, mf + ".bak", true);
        }
    }
}
