using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.ModBriefModule
{
    // 规格描述
    public record ModBriefSpecification;
    public record MS_Empty : ModBriefSpecification;
    public record MS_Category(string Category) : ModBriefSpecification;
    public record MS_Tag(string Tag) : ModBriefSpecification;
    public record MS_Blur(string Blur) : ModBriefSpecification;
    public record MS_And(ModBriefSpecification Left, ModBriefSpecification Right) : ModBriefSpecification;
    public record MS_Or(ModBriefSpecification Left, ModBriefSpecification Right) : ModBriefSpecification;

    public static class ModDetailSpecificationPackage
    {
        public static ModBriefSpecification And(this ModBriefSpecification left, ModBriefSpecification right)
        {
            return (left, right) switch
            {
                (MS_Empty, _) => right,
                (_, MS_Empty) => left,
                (_, _) => new MS_And(left, right)
            };
        }

        public static ModBriefSpecification Or(this ModBriefSpecification left, ModBriefSpecification right)
        {
            return (left, right) switch
            {
                (MS_Empty, _) => left,
                (_, MS_Empty) => right,
                (_, _) => new MS_Or(left, right)
            };
        }

        public static bool IsSpecified(this ModBriefSpecification spec, ModDetail md)
        {
            return spec switch
            {
                MS_Empty => true,
                MS_Category c => md.Categories.Contains(c.Category, StringComparison.OrdinalIgnoreCase),
                MS_Tag t => md.Tags.Contains(t.Tag, StringComparison.OrdinalIgnoreCase),
                MS_Blur b => md.VpkId.Id.ToString().Contains(b.Blur, StringComparison.OrdinalIgnoreCase) || md.Author.Contains(b.Blur, StringComparison.OrdinalIgnoreCase) || md.Name.Contains(b.Blur, StringComparison.OrdinalIgnoreCase),
                MS_And and => IsSpecified(and.Left, md) && IsSpecified(and.Right, md),
                MS_Or or => IsSpecified(or.Left, md) || IsSpecified(or.Right, md),
                _ => throw new NotImplementedException()
            };
        }
    };
}
