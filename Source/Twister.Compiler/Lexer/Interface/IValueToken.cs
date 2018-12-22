namespace Twister.Compiler.Lexer.Interface
{
    public interface IValueToken<T> : IToken
    {
        T Value { get; }
    }
}
