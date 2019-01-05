using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Parser.Interface
{
    public interface ITokenScanner
    {
        T ScanNext<T>() where T : IToken;

        T PeekNext<T>() where T : IToken;

        T PeekNext<T>(int count) where T : IToken;

        void ConsumeNext(TokenKind kind);

        void ConsumeNextPattern(TokenKind[] pattern);
    }
}
