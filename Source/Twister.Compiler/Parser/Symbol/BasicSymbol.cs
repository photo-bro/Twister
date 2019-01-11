using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Symbol
{
    public class BasicSymbol : ISymbol
    {
        public BasicSymbol(string identifier, SymbolKind kind, SymbolAttribute attributes, TwisterType dataType)
        {
            Identifier = identifier;
            Kind = kind;
            Attributes = attributes;
            DataType = dataType;
        }

        public string Identifier { get; private set; }

        public SymbolKind Kind { get; private set; }

        public SymbolAttribute Attributes { get; private set; }

        public TwisterType DataType { get; private set; }
    }
}
