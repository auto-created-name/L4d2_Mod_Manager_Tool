using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModLocalInfo
{
    [Table("mod")]
    internal class PO_LocalInfo
    {
        [Key]
        public int id { get;set; }
        public string thumbnail { get; set; }
        public string title { get; set; }
        public string version { get; set; }
        public string tagline { get; set; }
        public string author { get; set; }
        public string description { get; set; }
        public string categories { get; set; }
    }
}
