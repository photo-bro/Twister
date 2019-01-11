using System.Collections.Generic;
namespace Twister.Compiler.Parser.Interface
{
    public interface IScope
    {
        int Depth { get; }

        IList<ISymbol> Symbols { get; }

        IScope ParentScope { get; }

        IList<IScope> ChildScopes { get; }

        bool IsInScope(ISymbol symbol);

        bool IsInCurrentScope(ISymbol symbol);
    }
}