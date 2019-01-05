using System;
using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Interface
{
    public interface ISymbolNode<T> : IValueNode<T>
    {
        SymbolKind SymbolKind { get; }

        string Identifier { get; }
    }
}
