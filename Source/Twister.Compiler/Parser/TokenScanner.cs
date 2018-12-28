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

        public int Offset => throw new NotImplementedException();

        public int Base { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Position => throw new NotImplementedException();

        public int SourceLength => throw new NotImplementedException();

        public IEnumerable<IToken> CurrentWindow => throw new NotImplementedException();

        public char Advance()
        {
            throw new NotImplementedException();
        }

        public char Advance(int count)
        {
            throw new NotImplementedException();
        }

        public bool IsAtEnd()
        {
            throw new NotImplementedException();
        }

        public char Peek()
        {
            throw new NotImplementedException();
        }

        public char Peek(int count)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
