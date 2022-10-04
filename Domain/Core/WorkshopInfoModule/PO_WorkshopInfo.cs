using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.WorkshopInfoModule
{
    [Table("workshopinfo")]
    internal class PO_WorkshopInfo
    {
       [Key]
        public long vpk_id { get; set; }
        public string preview { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string tags { get; set; }
    }
}
