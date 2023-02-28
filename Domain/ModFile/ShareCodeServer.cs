using Domain.Core;
using Domain.Core.ModBriefModule;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System;

namespace Domain.ModFile
{
    /// <summary>
    /// 模组分享码服务
    /// </summary>
    public class ShareCodeServer
    {
        private ModBriefList modBriefList;
        public ShareCodeServer(ModBriefList modBriefList)
        {
            this.modBriefList = modBriefList;
        }

        /// <summary>
        /// 判断指定分享码是否合法
        /// </summary>
        /// <param name="code">分享码</param>
        /// <returns></returns>
        public static bool IsShareCodeValid(string code)
            => Regex.IsMatch(code, @"VPKID:([0-9;]+)");

        /// <summary>
        /// 生成分享码
        /// </summary>
        public string GenerateShareCode(IEnumerable<ModFile> mfs)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("-----------------------------------------------------");
            stringBuilder.AppendLine("我向你分享了求生之路2的模组，复制以下信息到求生之路2模组管理工具中批量订阅：");
            var infoMods = mfs.Where(mf => mf.VpkId != VpkId.Undefined).ToList();

            // 没有可以分享的模组，取消
            if (infoMods.Count == 0) 
                return "";

            // 生成可读文本
            infoMods.ForEach(mf => ReadableModInfo(mf, stringBuilder));
            stringBuilder.AppendLine();
            stringBuilder.Append("VPKID:");

            infoMods.ForEach(mf => stringBuilder.Append(mf.VpkId).Append(";"));

            //using MemoryStream ms = new();
            //using BinaryWriter bw = new(ms);
            //// 写入总长度
            //bw.Write(infoMods.Count);
            //// 写入具体ID
            //infoMods.ForEach(mf => bw.Write(mf.VpkId.Id));
            //var infoStr = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Position);
            //stringBuilder.AppendLine(infoStr);

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("-----------------------------------------------------");
            //var vpks = mfs.Select(x => x.VpkId).Where(x => x != VpkId.Undefined);
            //StringBuilder stringBuilder = new();
            //vpks.ToList().ForEach(v => stringBuilder.Append(v.Id).Append(';'));
            return stringBuilder.ToString();
        }

        public VpkId[] ParseShareCode(string code)
        {
            if(!IsShareCodeValid(code))
                return Array.Empty<VpkId>();
            //var match = Regex.Match(code, @"INFO:([a-zA-Z0-9\+\/]+=*)");
            var match = Regex.Match(code, @"VPKID:([0-9;]+)");
            try
            {
                if (match.Success)
                {
                    var content = match.Groups[1].Value;
                    return content.Split(';')
                        .Where(s => !string.IsNullOrEmpty(s))
                        .Select(long.Parse)
                        .Select(l => new VpkId(l))
                        .ToArray();
                    //var base64 = match.Groups[1].Value;
                    //var data = Convert.FromBase64String(base64);
                    //using var br = new BinaryReader(new MemoryStream(data));
                    //int length = br.ReadInt32();
                    //return Enumerable.Range(0, length).Select(i => br.ReadInt64()).Select(l => new VpkId(l)).ToArray();
                }
            }
            catch {
                return Array.Empty<VpkId>();
            }
            return Array.Empty<VpkId>();
        }

        private void ReadableModInfo(ModFile mf, StringBuilder builder)
        {
            builder
                .Append(modBriefList.GetById(mf.Id).Match(mb => mb.ReadableName, () => ""))
                .AppendLine();
        }
    }
}
