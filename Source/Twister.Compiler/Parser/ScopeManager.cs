using System;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser
{
    public class ScopeManager : IScopeManager
    {
        public ScopeManager()
        {
            ActiveScope = new Scope(null, 0);
            TotalDepth++;
        }

        public IScope ActiveScope { get; private set; }

        public int TotalDepth { get; private set; }

        public IScope NewScope()
        {
            var scope = new Scope(ActiveScope, TotalDepth++);
            ActiveScope = scope;
            return scope;
        }

        public void RemoveBottomScope()
        {
            ActiveScope = ActiveScope.ParentScope;
            TotalDepth--;
        }
    }
}
