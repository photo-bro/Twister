using Twister.Compiler.Lexer.Enum;

namespace Twister.Compiler.Lexer.Interface
{
    public interface IToken
    {
        TokenKind Kind { get; }

        int LineNumber { get; }
    }
}
