using Twister.Compiler.Lexer.Enum;

namespace Twister.Compiler.Lexer.Interface
{
    public interface IToken
    {
        TokenType Type { get; }

        int LineNumber { get; }
    }
}
