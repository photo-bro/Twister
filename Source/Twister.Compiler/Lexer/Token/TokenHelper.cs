using System.Collections.Generic;
using System.Linq;
using Twister.Compiler.Lexer.Enum;

namespace Twister.Compiler.Lexer.Token
{
    public static class TokenHelper
    {
        public static TokenKind[] ValueTokens => new TokenKind[]
        {
            TokenKind.SignedInt,
            TokenKind.UnsignedInt,
            TokenKind.Real,
            TokenKind.Identifier,
            TokenKind.Keyword,
            TokenKind.BoolLiteral,
            TokenKind.StringLiteral,
            TokenKind.CharLiteral,
            TokenKind.Operator
        };

        public static TokenKind[] PrimitiveTokens =
        {
            TokenKind.BoolLiteral,
            TokenKind.CharLiteral,
            TokenKind.SignedInt,
            TokenKind.UnsignedInt,
            TokenKind.Real,
            TokenKind.StringLiteral
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
            [Operator.BitNot] = "~",
            [Operator.LeftShift] = "<<",
            [Operator.RightShift] = ">>",
            [Operator.LogAnd] = "&&",
            [Operator.LogOr] = "||",
            [Operator.LogEqual] = "==",
            [Operator.LogNotEqual] = "!=",
            [Operator.LogNot] = "!",
            [Operator.LogLess] = "<",
            [Operator.LogGreater] = ">",
            [Operator.LogLessEqual] = "<=",
            [Operator.LogGreaterEqual] = ">="
        };

        public static Operator[] ConditionalOperators =
        {
            Operator.LogAnd,
            Operator.LogOr,
            Operator.LogEqual,
            Operator.LogNotEqual,
            Operator.LogNot,
            Operator.LogLess,
            Operator.LogGreater,
            Operator.LogLessEqual,
            Operator.LogGreaterEqual
        };

        public static Operator[] BinaryArithmeticOperators =
        {
            Operator.Plus,
            Operator.Minus,
            Operator.Modulo,
            Operator.Multiplication,
            Operator.ForwardSlash,
            Operator.BitAnd,
            Operator.BitOr,
            Operator.BitExOr,
            Operator.BitNot,
            Operator.LeftShift,
            Operator.RightShift
        };

        public static Operator[] UnaryArithmeticOperators =
        {
            Operator.Plus,
            Operator.Minus,
            Operator.BitNot
        };

        public static Keyword[] PrimitiveTypeKeywords =
        {
            Keyword.Bool,
            Keyword.Char,
            Keyword.Int,
            Keyword.UInt,
            Keyword.Float,
            Keyword.Str
        };

        public static bool IsValueToken(this TokenKind tokType) => ValueTokens.Any(vt => vt == tokType);

        public static bool IsPrimitive(this TokenKind tokType) => PrimitiveTokens.Any(t => t == tokType);

        public static bool IsConditionalOperator(this Operator @operator) => ConditionalOperators.Any(co => co == @operator);

        public static bool IsBinaryArithmeticOperator(this Operator @operator) => BinaryArithmeticOperators.Any(co => co == @operator);

        public static bool IsUnaryArithmeticOperator(this Operator @operator) => UnaryArithmeticOperators.Any(co => co == @operator);

        public static bool IsTypeKeyword(this Keyword keyword) => PrimitiveTypeKeywords.Any(kw => kw == keyword);
    }
}
