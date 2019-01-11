using System;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Node
{
    public class SymbolNode : IValueNode<ISymbol>
    {
        public SymbolNode(ISymbol value)
        {
            Value = value;
        }

        public ISymbol Value { get; private set; }

        public NodeKind Kind => NodeKind.Symbol;
    }
}
