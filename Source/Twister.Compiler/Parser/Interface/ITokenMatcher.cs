using System;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Parser.Interface
{
    public interface ITokenMatcher
    {
        T PeekNext<T>() where T : IToken;

        T PeekNext<T>(int count) where T : IToken;

        T MatchAndGet<T>() where T : IToken;

        T MatchAndGet<T>(Predicate<T> constraint) where T : IToken;

        void Match();

        void Match<T>() where T : IToken;

        void Match<T>(Predicate<T> constraint) where T : IToken;

        void MatchPattern(TokenKind[] pattern);
    }
}
