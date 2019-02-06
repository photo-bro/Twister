using System.Collections.Generic;

namespace Twister.Compiler.Parser.Interface
{
    public interface IScope
    {
        int Depth { get; }

        ICollection<ISymbol> Symbols { get; }

        IScope ParentScope { get; }

        IList<IScope> ChildScopes { get; }

        bool IsInScope(string identifier);

        bool IsInCurrentScope(string identifier);

        ISymbol GetSymbol(string identifier);

        void AddSymbol(ISymbol symbol);

        void AddSymbols(ICollection<ISymbol> symbols);
    }
}