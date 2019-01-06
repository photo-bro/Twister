using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Node
{
    public class SymbolNode : ISymbolNode<TwisterPrimitive>
    {
        public SymbolNode(SymbolKind kind, string identifier, TwisterPrimitive value)
        {
            SymbolKind = kind;
            Identifier = identifier;
            Value = value;
        }

        public SymbolKind SymbolKind { get; private set; }

        public string Identifier { get; private set; }

        public TwisterPrimitive Value { get; private set; }

        public NodeKind Kind => NodeKind.Symbol;

        public PrimitiveType Type => Value.Type;
    }
}
