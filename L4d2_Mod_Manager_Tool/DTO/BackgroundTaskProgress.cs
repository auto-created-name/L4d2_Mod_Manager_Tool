using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.DTO
{
    internal class BackgroundTaskProgress
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Progress { get; set; }
        public string Status {get;set;}
    }
}
