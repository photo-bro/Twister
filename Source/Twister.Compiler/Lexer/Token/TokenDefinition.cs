using System.Collections.Generic;
using System.Linq;

namespace Twister.Compiler.Lexer.Token
{
    public enum TokenType
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
        Keyword,

    }

    public enum Keyword
    {
        None = 0,
        If,
        Else,
        Func,
        Def,
        Struct,
        Return,
        While,
        Cont,
        Break,
        Int,
        UInt,
        Float,
        Str,
        Char,
        Bool
    }

    public enum Operator
    {
        None = 0,
        Plus,
        Minus,
        Modulo,
        Multiplication,
        ForwardSlash,
        BitAnd,
        BitOr,
        BitExOr,
        BitNot,
        LeftShift,
        RightShift,
        LogAnd,
        LogOr,
        LogEqual,
        LogNotEqual
    }


    public static class TokenHelper
    {
        public static TokenType[] ValueTokens => new TokenType[]
        {
            TokenType.SignedInt,
            TokenType.UnsignedInt,
            TokenType.Real,
            TokenType.Identifier,
            TokenType.Keyword,
            TokenType.BoolLiteral,
            TokenType.StringLiteral,
            TokenType.CharLiteral,
            TokenType.Operator
        };

        public static IReadOnlyDictionary<Operator, string> OperatorValueMap => new Dictionary<Operator, string>
        {
            [Operator.None] = string.Empty,
            [Operator.Plus] = "+",
            [Operator.Minus] = "-",
            [Operator.Modulo] = "%",
            [Operator.Multiplication] = "*",
            [Operator.ForwardSlash] = "/",
            [Operator.BitAnd] = "&",
            [Operator.BitOr] = "|",
            [Operator.BitExOr] = "^",
            [Operator.BitNot] = "!",
            [Operator.LeftShift] = "<<",
            [Operator.RightShift] = ">>",
            [Operator.LogAnd] = "&&",
            [Operator.LogOr] = "||",
            [Operator.LogEqual] = "==",
            [Operator.LogNotEqual] = "!=",
        };

        public static bool IsValueToken(this TokenType tokType) => ValueTokens.Any(vt => vt == tokType);

    }
}
