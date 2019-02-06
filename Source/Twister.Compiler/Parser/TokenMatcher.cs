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

        public IToken Current => _scanner.Peek(0);

        public IToken Peek => _scanner.Peek();

        public TokenMatcher(IScanner<IToken> scanner)
        {
            _scanner = scanner;
        }

        public bool IsNext<T>() where T : IToken
        {
            if (!(Peek is T))
                return false;

            return !Equals((T)Peek, default(T));
        }

        public bool IsNext<T>(Predicate<T> constraint) where T : IToken
        {
            if (!(Peek is T))
                return false;

            return constraint((T)Peek);
        }

        public IToken PeekNext(int count)
        {
            var peek = _scanner.Peek(count);
            return peek == _scanner.InvalidItem 
                ? default(IToken) 
                : peek;
        }

        public T MatchAndGet<T>() where T : IToken
        {
            var next = _scanner.Peek();
            if (!(next is T))
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
                if (k != Peek.Kind)
                    throw new UnexpectedTokenException("Unexpected token")
                    {
                        UnexpectedToken = Peek,
                        ExpectedTokenType = k.GetType()
                    };
                _scanner.Advance();
            }
        }

        public T MatchAndGetIfNext<T>() where T : IToken => IsNext<T>() ? MatchAndGet<T>() : default(T);

        public T MatchAndGetIfNext<T>(Predicate<T> constraint) where T : IToken => IsNext(constraint)
                                                                                       ? MatchAndGet(constraint)
                                                                                       : default(T);
    }
}
