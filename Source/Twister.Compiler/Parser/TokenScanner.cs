using System;
using System.Linq;
using System.Collections.Generic;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Common.Interface;
using Twister.Compiler.Lexer.Enum;

namespace Twister.Compiler.Parser
{
    public class TokenScanner : ITokenScanner, IScanner<IToken>
    {
        private IToken _invalidToken { get; } = new EmptyToken();
        private readonly Memory<IToken> _tokens;

        public TokenScanner(IEnumerable<IToken> tokens)
        {
            _tokens = tokens.ToArray().AsMemory();
        }

        public IToken InvalidItem => _invalidToken;

        public int Offset => Position - Base;

        public int Base { get; set; } = 0;

        public int Position { get; set; } = 0;

        public int SourceLength => _tokens.Length;

        /// <summary>
        /// Return current tokens in window. If at or past end of source tokens then an <see cref="IEnumerable{IToken}"/>
        /// with <see cref="InvalidItem"/> is returned.
        /// </summary>
        public IEnumerable<IToken> CurrentWindow
        {
            get
            {
                if (Base > SourceLength)
                    return new[] { _invalidToken };

                if (Base + Offset > SourceLength)
                    return _tokens.Slice(Base).ToArray();

                return _tokens.Slice(Base, Offset).ToArray();
            }
        }

        public IToken Advance() => Advance(1);

        public IToken Advance(int count)
        {
            if (IsAtEnd())
                return InvalidItem;

            if (Position + count > SourceLength)
                return InvalidItem;

            if (count == 0)
                return _tokens.Span[Position];

            if (count < 0)
                throw new InvalidOperationException($"{nameof(TokenScanner)}" +
                    $".{nameof(TokenScanner.Advance)} can only advance forward");

            var currentSpan = _tokens.Slice(Position, count).Span;

            Position += count;

            return currentSpan[count - 1];
        }

        public bool IsAtEnd()
        {
            if (SourceLength == 0) return true;
            return Position >= SourceLength;
        }

        public IToken Peek() => Peek(1);

        public IToken Peek(int count)
        {
            if (Position + count > SourceLength)
                return InvalidItem;

            if (Position + count == 0)
                return _tokens.Span[0];

            if (Position + count < 1)
                return InvalidItem;

            return _tokens.Span[Position + count - 1];
        }

        public void Reset()
        {
            Base = 0;
            Position = -1;
        }

        public T ScanNext<T>() where T : IToken
        {
            var next = Peek();
            var expectedNext = (T)next;
            if (next == null)
                throw new UnexpectedTokenException("Unexpected token")
                {
                    UnexpectedToken = next,
                    ExpectedTokenType = typeof(T)
                };

            return (T)Advance();
        }

        public T PeekNext<T>() where T : IToken => PeekNext<T>(1);

        public T PeekNext<T>(int count) where T : IToken
        {
            var peek = Peek(count);
            if (peek == InvalidItem)
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

        public void ConsumeNext(TokenKind kind)
        {
            var peek = Peek();
            if (peek.Kind != kind)
                throw new UnexpectedTokenException("Unexpected token")
                {
                    UnexpectedToken = peek,
                    ExpectedTokenType = kind.GetType()
                };

            Advance();
        }
        public void ConsumeNextPattern(TokenKind[] pattern)
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
                Advance();
            }
        }
    }
}
