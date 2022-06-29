using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace L4d2_Mod_Manager.Utility
{
    public static class FPExtension
    {
        public static Func<T, bool> Not<T>(Func<T, bool> f) => x => !f(x);

        /// <summary>
        /// 在序列间插入制定元素
        /// </summary>
        public static IEnumerable<T> Intersperse<T>(this IEnumerable<T> source, T element) {
            bool first = true;
            foreach (T value in source) 
            { 
                if (!first) 
                    yield return element;
                yield return value;
                first = false; 
            }
        }


        public static Maybe<TOut> FindElementSafe<T, TOut>(this ILookup<T,TOut> lk, T key)
        {
            if (lk.Contains(key))
            {
                return Maybe.Some(lk[key].First());
            }
            else
            {
                return Maybe.None;
            }
        }
    }
}
