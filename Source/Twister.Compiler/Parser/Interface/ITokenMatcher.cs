using Twister.Compiler.Common.Interface;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Parser.Interface
{
    public interface ITokenMatcher : IMatcher<IToken>
    {      
        void MatchPattern(TokenKind[] pattern);
    }
}
