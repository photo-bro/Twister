namespace Twister.Compiler.Lexer.Enum
{
    public enum TokenKind
    {
        None = 0,          // default

        Operator,

        Define,
        Assign,
        Colon,
        Semicolon,
        Comma,
        Dot,
        DotDot,
        QuestionMark,

        // Brackets
        LeftParen,
        RightParen,
        LeftBrack,
        RightBrack,
        LeftSquareBrack,
        RightSquareBrack,
        LessThan,
        GreaterThan,

        SignedInt,
        UnsignedInt,
        Real,

        BoolLiteral,
        StringLiteral,
        CharLiteral,
        Identifier,
        Keyword
    }
}
