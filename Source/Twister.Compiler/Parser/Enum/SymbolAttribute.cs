using System;

namespace Twister.Compiler.Parser.Enum
{
    [Flags]
    public enum SymbolAttribute
    {
        None = 0,
        Local = 1 << 0,
        Reference = 1 << 2,
        FuncParam = 1 << 3
    }
}