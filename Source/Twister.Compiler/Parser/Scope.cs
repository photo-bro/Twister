using System;
using System.Collections.Generic;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser
{
    public class Scope : IScope
    {
        public Scope(IScope parent, int depth)
        {
            ParentScope = parent;
            Depth = depth;
            Symbols = new List<ISymbol>();
            ChildScopes = new List<IScope>();
        }

        public int Depth { get; private set; }

        public IList<ISymbol> Symbols { get; set; }

        public IScope ParentScope { get; private set; }

        public IList<IScope> ChildScopes { get; set; }

        public bool IsInCurrentScope(ISymbol symbol) => Symbols.Contains(symbol);

        public bool IsInScope(ISymbol symbol) => IsInCurrentScope(symbol) || (ParentScope?.IsInScope(symbol) ?? false);
    }
}