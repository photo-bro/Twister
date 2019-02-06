using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Symbol
{
    public class BasicSymbol : ISymbol
    {
        public BasicSymbol(string identifier, SymbolKind kind, SymbolAttribute attributes,
            TwisterType dataType, object value)
        {
            Identifier = identifier;
            Kind = kind;
            Attributes = attributes;
            DataType = dataType;
            Value = value;
        }

        public string Identifier { get; }

        public SymbolKind Kind { get; }

        public SymbolAttribute Attributes { get; }

        public TwisterType DataType { get; }

        public object Value { get; }
    }
}
