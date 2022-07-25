using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    public interface ISpecification<out T>
    {
        string ToSqlite();
    }

    class EmptySpecification<T> : ISpecification<T>
    {
        public string ToSqlite() => "";
    }

    public static class Specification
    {
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return (left, right) switch
            {
                (EmptySpecification<T>, _) => right,
                (_, EmptySpecification<T>) => left,
                _ => new AndSpecification<T>(left, right)
            };
        }
        public static ISpecification<T> Empty<T>() => new EmptySpecification<T>();
    }
}
