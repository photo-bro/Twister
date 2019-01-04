using System;
using System.Linq;
using System.Collections.Generic;
using Twister.Compiler.Common.Interface;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
namespace Twister.Compiler.Parser
{
    public class TokenScanner : IScanner<IToken>
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
    }
}
