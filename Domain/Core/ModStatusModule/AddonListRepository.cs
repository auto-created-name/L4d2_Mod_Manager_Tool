using Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Core.ModStatusModule
{
    public class AddonListRepository
    {
        private Dictionary<string, bool> status = new();

        public AddonListRepository()
        {
            Load();
        }

        public Maybe<bool> this [string mod]
            => GetModStatus(mod);

        public Maybe<bool> GetModStatus(string mod)
            => status.ContainsKey(mod) ? status[mod] : Maybe.None;

        public void SetModStatus(string mod, bool b)
            => status[mod] = b;

        #region 读取/写入数据

        /// <summary>
        /// 读取addonlist文件
        /// </summary>
        public void Load()
        {
            status.Clear(); 
            var als = ParseAddonList(ReadAddonListContent());
            als.ForEach(x => status.Add(x.Item1, x.Item2));
        }

        private static string AddonListFilePath
            => Path.Combine(L4d2Folder.CoreFolder, "addonlist.txt");

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
        private static List<(string, bool)> ParseAddonList(string content)
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


        /// <summary>
        /// 保存修改到addonlist文件
        /// </summary>
        public void Save()
        {
            ApplyAddonList();
        }

        private void ApplyAddonList()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("\"AddonList\"").AppendLine("{");
            foreach(var pair in status)
            {
                var enableStr = pair.Value ? "1" : "0";
                stringBuilder.AppendLine($"\t\"{pair.Key}\"\t\t\"{enableStr}\"");
            }
            stringBuilder.AppendLine("}");

            WriteAddonListContent(stringBuilder.ToString());
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
            }
            catch 
            {
                throw;
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
        #endregion
    }
}
