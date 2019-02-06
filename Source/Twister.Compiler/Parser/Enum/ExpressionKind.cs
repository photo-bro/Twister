namespace Twister.Compiler.Parser.Enum
{
    public enum ExpressionKind
    {
        General = 0,
        Conditional,
        Unary,
        Additive,
        Multiplicative,
        Shift,
        Relational,
        Equality,
        BitAnd,
        BitExOr,
        BitOr,
        LogAnd,
        LogOr
    }
}
