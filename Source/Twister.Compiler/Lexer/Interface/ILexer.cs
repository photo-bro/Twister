using System.Collections.Generic;

namespace Twister.Compiler.Lexer.Interface
{
    public interface ILexer
    {
        IEnumerable<IToken> Tokenize(string sourceCode, LexerFlag flags);
    }
}
