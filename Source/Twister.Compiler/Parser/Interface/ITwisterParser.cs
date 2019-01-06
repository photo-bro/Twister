using System.Collections.Generic;
using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Parser.Interface
{
    public interface ITwisterParser
    {
        INode Parse(IEnumerable<IToken> twisterTokens);
    }
}
