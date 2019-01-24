using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Interface
{
    public interface ISymbol
    {
        string Identifier { get; }

        SymbolKind Kind { get; }

        SymbolAttribute Attributes { get; }

        TwisterType DataType { get; }

        object Value { get; }
    }
}
