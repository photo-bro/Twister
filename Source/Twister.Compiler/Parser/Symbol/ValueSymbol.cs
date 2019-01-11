using System;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Symbol
{
    public class ValueSymbol<T> : IValueSymbol<T>
    {
        public ValueSymbol(string name, T value, SymbolKind kind, SymbolAttribute attributes, TwisterType type)
        {
            Identifier = name;
            Value = value;
            Kind = kind;
            Attributes = attributes;
            DataType = type;
        }

        public T Value { get; private set; }

        public string Identifier { get; private set; }

        public SymbolKind Kind { get; private set; }

        public SymbolAttribute Attributes { get; private set; }

        public TwisterType DataType { get; private set; }
    }
}
