namespace Twister.Compiler.Parser.Interface
{
    public interface IValueSymbol<T> : ISymbol
    {
        T Value { get; }
    }
}