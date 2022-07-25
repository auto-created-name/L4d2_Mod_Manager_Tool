using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    class AndSpecification<T> : ISpecification<T>
    {
        private ISpecification<T> left;
        private ISpecification<T> right;
        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        //public bool IsSatisifiedBy(T o)
        //{
        //    return left.IsSatisifiedBy(o) && right.IsSatisifiedBy(o);
        //}

        public string ToSqlite()
        {
            if(left is EmptySpecification<T>)
                return right.ToSqlite();
            else if(right is EmptySpecification<T>)
                return left.ToSqlite();
            else
                return $"{left.ToSqlite()} AND {right.ToSqlite()}";
        }
    }
}
