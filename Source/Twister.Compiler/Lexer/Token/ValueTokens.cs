using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Lexer.Token
{
    public class SignedIntToken : BaseToken, IValueToken<long>
    {
        public long Value { get; set; }
        public override TokenType Type => TokenType.SignedInt;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class UnsignedIntToken : BaseToken, IValueToken<ulong>
    {
        public ulong Value { get; set; }
        public override TokenType Type => TokenType.UnsignedInt;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class RealToken : BaseToken, IValueToken<double>
    {
        public double Value { get; set; }
        public override TokenType Type => TokenType.Real;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class IdToken : BaseToken, IValueToken<string>
    {
        public string Value { get; set; }
        public override TokenType Type => TokenType.Identifier;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class KeywordToken : BaseToken, IValueToken<Keyword>
    {
        public Keyword Value { get; set; }
        public override TokenType Type => TokenType.Keyword;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class OperatorToken : BaseToken, IValueToken<Operator>
    {
        public Operator Value { get; set; }
        public override TokenType Type => TokenType.Operator;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class StringLiteralToken : BaseToken, IValueToken<string>
    {
        public string Value { get; set; }
        public override TokenType Type => TokenType.StringLiteral;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class CharLiteralToken : BaseToken, IValueToken<char>
    {
        public char Value { get; set; }
        public override TokenType Type => TokenType.StringLiteral;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }
}
