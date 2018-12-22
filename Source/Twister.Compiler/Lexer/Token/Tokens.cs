using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Lexer.Token
{
    public struct Token : IToken
    {
        public TokenType Type { get; set; }
        public int LineNumber { get; set; }
    }

    public struct EmptyToken : IToken
    {
        public TokenType Type => TokenType.None;
        public int LineNumber { get; set; }
    }

    public struct DefineToken : IToken
    {
        public TokenType Type => TokenType.Define;
        public int LineNumber { get; set; }
    }

    public struct AssignToken : IToken
    {
        public TokenType Type => TokenType.Assign;
        public int LineNumber { get; set; }
    }

    public struct ColonToken : IToken
    {
        public TokenType Type => TokenType.Colon;
        public int LineNumber { get; set; }
    }

    public struct SemiColonToken : IToken
    {
        public TokenType Type => TokenType.Semicolon;
        public int LineNumber { get; set; }
    }

    public struct CommaToken : IToken
    {
        public TokenType Type => TokenType.Comma;
        public int LineNumber { get; set; }
    }

    public struct DotToken : IToken
    {
        public TokenType Type => TokenType.Dot;
        public int LineNumber { get; set; }
    }

    public struct DotDotToken : IToken
    {
        public TokenType Type => TokenType.DotDot;
        public int LineNumber { get; set; }
    }

    public struct QuestionMarkToken : IToken
    {
        public TokenType Type => TokenType.QuestionMark;
        public int LineNumber { get; set; }
    }

    public struct LeftParenToken : IToken
    {
        public TokenType Type => TokenType.LeftParen;
        public int LineNumber { get; set; }
    }

    public struct RightParenToken : IToken
    {
        public TokenType Type => TokenType.RightParen;
        public int LineNumber { get; set; }
    }

    public struct LeftBrackToken : IToken
    {
        public TokenType Type => TokenType.LeftBrack;
        public int LineNumber { get; set; }
    }

    public struct RightBrackToken : IToken
    {
        public TokenType Type => TokenType.RightBrack;
        public int LineNumber { get; set; }
    }

    public struct LeftSquareBrackToken : IToken
    {
        public TokenType Type => TokenType.LeftSquareBrack;
        public int LineNumber { get; set; }
    }

    public struct RightSquareBrackToken : IToken
    {
        public TokenType Type => TokenType.RightSquareBrack;
        public int LineNumber { get; set; }
    }

    public struct LessThan : IToken
    {
        public TokenType Type => TokenType.LessThan;
        public int LineNumber { get; set; }
    }

    public struct GreaterThan : IToken
    {
        public TokenType Type => TokenType.GreaterThan;
        public int LineNumber { get; set; }
    }
}
