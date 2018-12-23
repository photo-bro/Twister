using System.Collections.Generic;

namespace Twister.Compiler.Lexer.Interface
{
    public interface ILexer
    {
        IEnumerable<IToken> LexicalAnalysis(string sourceCode, LexerFlag flags);
    }
}
