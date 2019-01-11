namespace Twister.Compiler.Parser.Interface
{
    public interface ISymbolTable
    {
        int ScopeDepth { get; }

        IScope TopScope { get; }

        IScope ActiveScope { get; }
    }
}
