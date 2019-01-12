using System.Collections.Generic;
using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Parser.Interface
{
    public interface ITwisterParser
    {
        INode ParseProgram(IEnumerable<IToken> twisterTokens);

        INode ParseExpression(IEnumerable<IToken> twisterTokens);
    }
}
