using Twister.Compiler.Common.Interface;
namespace Twister.Compiler.Lexer.Interface
{
    public interface ISourceScanner : IScanner<char>
    {
        int CurrentSourceLine { get; }

        new string CurrentWindow { get; }
    }
}
