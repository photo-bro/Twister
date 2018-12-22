using System.Collections.Generic;

namespace Twister.Compiler.Lexer.Interface
{
    public interface ILexer
    {
        IList<IToken> LexicalAnalysis(string sourceCode, LexerFlag flags);
    }
}
