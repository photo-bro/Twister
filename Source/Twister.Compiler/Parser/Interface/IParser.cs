using System.Collections.Generic;
using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Parser.Interface
{
    public interface IParser
    {
        INode ParseTokensToNodes(IEnumerable<IToken> tokens);
    }
}
