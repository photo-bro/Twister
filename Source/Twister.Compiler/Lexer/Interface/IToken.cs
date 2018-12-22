using Twister.Compiler.Lexer.Token;

namespace Twister.Compiler.Lexer.Interface
{
    public interface IToken
    {
        TokenType Type { get; }

        int LineNumber { get; }
    }
}
