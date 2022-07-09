using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain
{
    public record ModDetail(
    int Id, string Img, string Name,
    string Vpkid, string Author,
    string Tagline, string Categories,
    string Tags, string Descript);

    public class ModDetailNameComparer : IComparer<ModDetail>
    {
        public int Compare(ModDetail x, ModDetail y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
