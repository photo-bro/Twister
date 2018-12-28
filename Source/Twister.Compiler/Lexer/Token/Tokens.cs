using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Enum;

namespace Twister.Compiler.Lexer.Token
{
    public abstract class BaseToken : IToken
    {
        public abstract TokenType Type { get; }
        public int LineNumber { get; set; }

        public override string ToString() => $"Line: {LineNumber}; TokenType: {Type};";
    }

    public class EmptyToken : BaseToken
    {
        public override TokenType Type => TokenType.None;
    }

    public class DefineToken : BaseToken
    {
        public override TokenType Type => TokenType.Define;
    }

    public class AssignToken : BaseToken
    {
        public override TokenType Type => TokenType.Assign;
    }

    public class ColonToken : BaseToken
    {
        public override TokenType Type => TokenType.Colon;
    }

    public class SemiColonToken : BaseToken
    {
        public override TokenType Type => TokenType.Semicolon;
    }

    public class CommaToken : BaseToken
    {
        public override TokenType Type => TokenType.Comma;
    }

    public class DotToken : BaseToken
    {
        public override TokenType Type => TokenType.Dot;
    }

    public class DotDotToken : BaseToken
    {
        public override TokenType Type => TokenType.DotDot;
    }

    public class QuestionMarkToken : BaseToken
    {
        public override TokenType Type => TokenType.QuestionMark;
    }

    public class LeftParenToken : BaseToken
    {
        public override TokenType Type => TokenType.LeftParen;
    }

    public class RightParenToken : BaseToken
    {
        public override TokenType Type => TokenType.RightParen;
    }

    public class LeftBrackToken : BaseToken
    {
        public override TokenType Type => TokenType.LeftBrack;
    }

    public class RightBrackToken : BaseToken
    {
        public override TokenType Type => TokenType.RightBrack;
    }

    public class LeftSquareBrackToken : BaseToken
    {
        public override TokenType Type => TokenType.LeftSquareBrack;
    }

    public class RightSquareBrackToken : BaseToken
    {
        public override TokenType Type => TokenType.RightSquareBrack;
    }

    public class LessThan : BaseToken
    {
        public override TokenType Type => TokenType.LessThan;
    }

    public class GreaterThan : BaseToken
    {
        public override TokenType Type => TokenType.GreaterThan;
    }
}
