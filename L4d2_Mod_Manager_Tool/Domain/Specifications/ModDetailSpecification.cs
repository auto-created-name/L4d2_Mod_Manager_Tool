using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Specifications
{
    // 规格描述
    public record ModDetailSpecification;
    public record MS_Empty : ModDetailSpecification;
    public record MS_Category(string Category) : ModDetailSpecification;
    public record MS_Tag(string Tag) : ModDetailSpecification;
    public record MS_Blur(string Blur) : ModDetailSpecification;
    public record MS_And(ModDetailSpecification Left, ModDetailSpecification Right) : ModDetailSpecification;
    public record MS_Or(ModDetailSpecification Left, ModDetailSpecification Right) : ModDetailSpecification;

    public static class ModDetailSpecificationPackage
    {
        public static ModDetailSpecification And(this ModDetailSpecification left, ModDetailSpecification right)
        {
            return (left, right) switch
            {
                (MS_Empty, _) => right,
                (_, MS_Empty) => left,
                (_, _) => new MS_And(left, right)
            };
        }

        public static ModDetailSpecification Or(this ModDetailSpecification left, ModDetailSpecification right)
        {
            return (left, right) switch
            {
                (MS_Empty, _) => left,
                (_, MS_Empty) => right,
                (_, _) => new MS_Or(left, right)
            };
        }
    };
}
