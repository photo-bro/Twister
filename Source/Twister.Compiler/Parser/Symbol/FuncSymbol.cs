using System;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Symbol
{
    public class FuncSymbol : ISymbol
    {
        public FuncSymbol(string identifier, TwisterType type, SymbolAttribute attributes, object value)
        {
            Identifier = identifier;
            DataType = type;
            Attributes = attributes;
            Value = value;
        }

        public string Identifier { get; private set; }

        public SymbolKind Kind => SymbolKind.Function;

        public SymbolAttribute Attributes { get; private set; }

        public TwisterType DataType { get; private set; }

        public object Value { get; private set; }
    }
}
