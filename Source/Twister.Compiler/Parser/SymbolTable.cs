using System.Collections.Generic;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser
{
    public class SymbolTable : ISymbolTable
    {
        private Stack<IScope> _scopeStack;

        public SymbolTable(IScope topScope)
        {
            TopScope = topScope;
            _scopeStack = new Stack<IScope>();
            _scopeStack.Push(topScope);

        }

        public int ScopeDepth { get; } = 0;

        public IScope TopScope { get; }

        public IScope ActiveScope => _scopeStack.Peek();
    }
}
