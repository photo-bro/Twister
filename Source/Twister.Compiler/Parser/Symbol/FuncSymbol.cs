using System;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Symbol
{
    public class FuncSymbol : ISymbol
    {
       public string Identifier => throw new NotImplementedException();

        public SymbolKind Kind => throw new NotImplementedException();

        public SymbolAttribute Attributes => throw new NotImplementedException();

        public TwisterType DataType => throw new NotImplementedException();

        public object Value => throw new NotImplementedException();
    }
}
