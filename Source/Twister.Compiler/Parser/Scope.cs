using System.Linq;
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

        public int Depth { get; }

        public ICollection<ISymbol> Symbols { get; set; }

        public IScope ParentScope { get; }

        public IList<IScope> ChildScopes { get; set; }

        public bool IsInCurrentScope(string identifier) => Symbols.Any(s => s.Identifier == identifier);

        public bool IsInScope(string identifier) =>
            IsInCurrentScope(identifier) || (ParentScope?.IsInScope(identifier) ?? false);

        public ISymbol GetSymbol(string identifier)
        {
            var symbol = Symbols.SingleOrDefault(s => s.Identifier == identifier);
            if (symbol == null)
                throw new UndefinedSymbolException(string.Empty)
                {
                    Identifier = identifier
                };
            return symbol;
        }

        public void AddSymbol(ISymbol symbol)
        {
            if (!IsInCurrentScope(symbol.Identifier))
                Symbols.Add(symbol);

            throw new DuplicateDefinitionException(string.Empty)
            {
                Identifier = symbol.Identifier
            };
        }

        public void AddSymbols(ICollection<ISymbol> symbols)
        {
            foreach (var s in symbols)
                AddSymbol(s);
        }
    }
}