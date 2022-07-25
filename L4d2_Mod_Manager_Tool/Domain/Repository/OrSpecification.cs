using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    class OrSpecification<T> : ISpecification<T>
    {
        private ISpecification<T> left;
        private ISpecification<T> right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public string ToSqlite()
        {
            return $"({left.ToSqlite()} OR {right.ToSqlite()})";
        }
    }
}
