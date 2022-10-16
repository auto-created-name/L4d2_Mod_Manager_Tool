using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 实体基类
    /// </summary>
    /// <typeparam name="T">标识类型</typeparam>
    public abstract class Entity<T>
    {
        public T Id { get; private set; }
        public Entity(T id)
        {
            Id = id;
        }
    }
}
