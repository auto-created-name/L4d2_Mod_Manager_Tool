using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace Domain.ModFile
{
    [Table("mod_file")]
    class PO_ModFile
    {
        [Key]
        public int id { get; set; }
        public long vpk_id { get; set; }
        public string file_name { get; set; }
        public int localinfo_id { get; set; }
    }
}
