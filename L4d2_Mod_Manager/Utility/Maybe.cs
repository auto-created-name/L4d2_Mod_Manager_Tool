﻿using System;
using System.Collections.Generic;
using System.Text;

namespace L4d2_Mod_Manager.Utility
{
    public struct Maybe<T>
    {
        private readonly T value;

        private readonly bool hasValue;

        private Maybe(T value)
        {
            this.value = value;
            hasValue = true;
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            if (hasValue)
                return some(value);

            return none();
        }

        public void Match(Action<T> some, Action none)
        {
            if (hasValue)
            {
                some(value);
            }
            else
            {
                none();
            }
        }

        public static implicit operator Maybe<T>(T value)
        {
            if (value == null)
                return new Maybe<T>();

            return new Maybe<T>(value);
        }

        public static implicit operator Maybe<T>(Maybe.MaybeNone value)
        {
            return new Maybe<T>();
        }

        public bool TryGetValue(out T value)
        {
            if (hasValue)
            {
                value = this.value;
                return true;
            }

            value = default(T);
            return false;
        }

        public Maybe<TResult> Map<TResult>(Func<T, TResult> convert)
        {
            if (!hasValue)
                return new Maybe<TResult>();

            return convert(value);
        }

        public void Map(Action<T> a)
        {
            if (hasValue)
                a(value);
        }

        public Maybe<TResult> Select<TResult>(Func<T, TResult> convert)
        {
            if (!hasValue)
                return new Maybe<TResult>();

            return convert(value);
        }

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> convert)
        {
            if (!hasValue)
                return new Maybe<TResult>();

            return convert(value);
        }

        public Maybe<TResult> SelectMany<T2, TResult>(
            Func<T, Maybe<T2>> convert,
            Func<T, T2, TResult> finalSelect)
        {
            if (!hasValue)
                return new Maybe<TResult>();

            var converted = convert(value);

            if (!converted.hasValue)
                return new Maybe<TResult>();

            return finalSelect(value, converted.value);
        }

        public Maybe<T> Where(Func<T, bool> predicate)
        {
            if (!hasValue)
                return new Maybe<T>();

            if (predicate(value))
                return this;

            return new Maybe<T>();
        }

        public T ValueOr(T defaultValue)
        {
            if (hasValue)
                return value;

            return defaultValue;
        }

        //public T ValueOr(Func<T> defaultValueFactory)
        //{
        //    if (hasValue)
        //        return value;
        //
        //    return defaultValueFactory();
        //}

        public Maybe<T> ValueOrMaybe(Maybe<T> alternativeValue)
        {
            if (hasValue)
                return this;

            return alternativeValue;
        }

        public Maybe<T> ValueOrMaybe(Func<Maybe<T>> alternativeValueFactory)
        {
            if (hasValue)
                return this;

            return alternativeValueFactory();
        }

        public T ValueOrThrow(string errorMessage)
        {
            if (hasValue)
                return value;

            throw new Exception(errorMessage);
        }
    }

    public static class Maybe
    {
        public class MaybeNone
        {
        }

        public static MaybeNone None { get; } = new MaybeNone();

        public static Maybe<T> Some<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return value;
        }
    }
}
