using System;
namespace Twister.Compiler.Common.Interface
{
    public interface IMatcher<T>
    {
        bool IsNext<U>() where U : T;

        bool IsNext<U>(Predicate<U> constraint) where U : T;

        T PeekNext(int count);

        U MatchAndGet<U>() where U : T;

        U MatchAndGet<U>(Predicate<U> constraint) where U : T;

        U MatchAndGetIfNext<U>() where U : T;

        U MatchAndGetIfNext<U>(Predicate<U> constraint) where U : T;

        void Match();

        void Match<U>() where U : T;

        void Match<U>(Predicate<U> constraint) where U : T;

        T Current { get; }

        T Peek { get; }
    }
}
