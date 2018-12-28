using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Enum;

namespace Twister.Compiler.Lexer.Token
{
    public abstract class BaseToken : IToken
    {
        public abstract TokenKind Kind { get; }
        public int LineNumber { get; set; }

        public override string ToString() => $"Line: {LineNumber}; TokenType: {Kind};";
    }

    public class EmptyToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.None;
    }

    public class DefineToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.Define;
    }

    public class AssignToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.Assign;
    }

    public class ColonToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.Colon;
    }

    public class SemiColonToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.Semicolon;
    }

    public class CommaToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.Comma;
    }

    public class DotToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.Dot;
    }

    public class DotDotToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.DotDot;
    }

    public class QuestionMarkToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.QuestionMark;
    }

    public class LeftParenToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.LeftParen;
    }

    public class RightParenToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.RightParen;
    }

    public class LeftBrackToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.LeftBrack;
    }

    public class RightBrackToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.RightBrack;
    }

    public class LeftSquareBrackToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.LeftSquareBrack;
    }

    public class RightSquareBrackToken : BaseToken
    {
        public override TokenKind Kind => TokenKind.RightSquareBrack;
    }

    public class LessThan : BaseToken
    {
        public override TokenKind Kind => TokenKind.LessThan;
    }

    public class GreaterThan : BaseToken
    {
        public override TokenKind Kind => TokenKind.GreaterThan;
    }
}
