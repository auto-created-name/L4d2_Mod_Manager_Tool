using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    public interface ISpecification<T>
    {
        bool IsSatisifiedBy(T o);
    }

    public static class SpecificationExtend
    {
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
            => new AndSpecification<T>(left, right);
    }
}
