using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Common.Interface;
using Twister.Compiler.Lexer.Enum;
using System;

namespace Twister.Compiler.Parser
{
    public class TokenMatcher : ITokenMatcher
    {
        private readonly IScanner<IToken> _scanner;

        public TokenMatcher(IScanner<IToken> scanner)
        {
            _scanner = scanner;
        }

        public T PeekNext<T>() where T : IToken => PeekNext<T>(1);

        public T PeekNext<T>(int count) where T : IToken
        {
            var peek = _scanner.Peek(count);
            if (peek == _scanner.InvalidItem)
                return default(T);

            var expectedNext = (T)peek;
            if (peek == null)
                throw new UnexpectedTokenException("Unexpected token")
                {
                    UnexpectedToken = peek,
                    ExpectedTokenType = typeof(T)
                };

            return expectedNext;
        }

        public T MatchAndGet<T>() where T : IToken
        {
            var next = _scanner.Peek();
            var expectedNext = (T)next;
            if (next == null)
                throw new UnexpectedTokenException("Unexpected token")
                {
                    UnexpectedToken = next,
                    ExpectedTokenType = typeof(T)
                };

            return (T)_scanner.Advance();
        }

        public T MatchAndGet<T>(Predicate<T> constraint) where T : IToken
        {
            var next = MatchAndGet<T>();
            if (!constraint(next))
                throw new UnexpectedTokenException("Token failed constraint")
                { UnexpectedToken = next };

            return next;
        }

        public void Match() => _scanner.Advance();

        public void Match<T>() where T : IToken => MatchAndGet<T>();

        public void Match<T>(Predicate<T> constraint) where T : IToken => MatchAndGet(constraint);

        public void MatchPattern(TokenKind[] pattern)
        {
            foreach (var k in pattern)
            {
                var actual = PeekNext<IToken>();
                if (k != actual.Kind)
                    throw new UnexpectedTokenException("Unexpected token")
                    {
                        UnexpectedToken = actual,
                        ExpectedTokenType = k.GetType()
                    };
                _scanner.Advance();
            }
        }
    }
}
