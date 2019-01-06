using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Node
{
    public class SymbolNode : ISymbolNode<TwisterPrimitive>
    {
        public SymbolNode(SymbolKind kind, string identifier, IExpressionNode<TwisterPrimitive> expression)
        {
            SymbolKind = kind;
            Identifier = identifier;
            Value = expression.Value;
        }

        public SymbolKind SymbolKind { get; private set; }

        public string Identifier { get; private set; }

        public TwisterPrimitive Value { get; private set; }

        public NodeKind Kind => NodeKind.Symbol;
    }
}
