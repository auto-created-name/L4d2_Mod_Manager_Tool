﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Utility
{
    public static class FPExtension
    {
        public static Func<T, bool> Not<T>(Func<T, bool> f) => x => !f(x);

        public static T Identity<T>(T t) => t;

        public static IEnumerable<T> Pure<T>(T element)
        {
            yield return element;   
        }

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

        public static void Iter<T>(this IEnumerable<T> xs, Action<T> a)
        {
            foreach (var x in xs)
                a(x);
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

        /// <summary>
        /// 安全地获取序列中的第一个元素
        /// </summary>
        public static Maybe<T> FirstElementSafe<T>(this IEnumerable<T> xs)
        {
            return xs.Any() ? xs.First() : Maybe.None;
        }

        public static IEnumerable<T> DiscardMaybe<T>(this IEnumerable<Maybe<T>> xs)
        {
            foreach(var x in xs)
            {
                if (x.HasValue)
                    yield return x.ValueOrThrow("");
            }
        }
    }
}
