using System.Collections.Generic;
using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Parser.Interface
{
    public interface ITwisterGrammarParser
    {
        INode Parse(IEnumerable<IToken> tokens, ParserFlag flags);
    }

    internal interface IAssignmentParser
    {

    }

    internal interface IExpressionParser
    {

    }
}
