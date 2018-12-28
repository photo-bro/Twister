using System.Collections.Generic;
using System.Linq;
using Twister.Compiler.Lexer.Enum;

namespace Twister.Compiler.Lexer.Token
{
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
