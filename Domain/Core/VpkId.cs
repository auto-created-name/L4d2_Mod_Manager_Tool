using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Core
{
    public record VpkId(long Id)
    {
        public static VpkId Undefined => new(0);

        /// <summary>
        /// 尝试从文件名中解析vpkid
        /// </summary>
        public static VpkId TryParse(string file)
        {
            var fn = Path.GetFileNameWithoutExtension(file);
            return IsVpkId(fn) ? new VpkId(long.Parse(fn)) : Undefined;
        }

        public static bool IsVpkId(string str)
            => Regex.IsMatch(str, @"^[1-9]?[0-9]{9}$");
    };
}
