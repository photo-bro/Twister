namespace Twister.Compiler.Parser.Enum
{
    enum PrecedenceLevel
    {
        Primitive = 0,
        Unary,
        Multiplicative,
        Addition,
        Shift,
        Relational,
        Equality,
        BitAnd,
        BitExor,
        BitOr,
        LogAnd,
        LogOr
    }
}
