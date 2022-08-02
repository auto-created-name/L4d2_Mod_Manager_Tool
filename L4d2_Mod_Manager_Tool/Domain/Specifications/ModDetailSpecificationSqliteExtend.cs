using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Specifications
{
    public static class ModDetailSpecificationSqliteExtend
    {
        public static string ToSqlite(this ModDetailSpecification spec)
        {
            return spec switch
            {
                MS_Empty    => "",
                MS_Category => $"categories like '%{(spec as MS_Category).Category}%'",
                MS_Tag      => $"workshop_tags like '%{(spec as MS_Tag).Tag}%'",
                MS_Blur blur => string.Format("(title like '%{0}%' OR workshop_title like '%{0}%' OR author like '%{0}%' OR vpk_id like '{0}%')", blur.Blur),
                MS_And and  => and.Left.ToSqlite() + " AND " + and.Right.ToSqlite(),
                MS_Or or    => $"({or.Left.ToSqlite()} OR {or.Right.ToSqlite()})",
                _           => throw new InvalidOperationException()
            };
        }
    }
}
