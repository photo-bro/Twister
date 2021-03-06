﻿using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Enum;

namespace Twister.Compiler.Lexer.Token
{
    public class SignedIntToken : BaseToken, IValueToken<long>
    {
        public long Value { get; set; }
        public override TokenKind Kind => TokenKind.SignedInt;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class UnsignedIntToken : BaseToken, IValueToken<ulong>
    {
        public ulong Value { get; set; }
        public override TokenKind Kind => TokenKind.UnsignedInt;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class RealToken : BaseToken, IValueToken<double>
    {
        public double Value { get; set; }
        public override TokenKind Kind => TokenKind.Real;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class IdToken : BaseToken, IValueToken<string>
    {
        public string Value { get; set; }
        public override TokenKind Kind => TokenKind.Identifier;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class KeywordToken : BaseToken, IValueToken<Keyword>
    {
        public Keyword Value { get; set; }
        public override TokenKind Kind => TokenKind.Keyword;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class OperatorToken : BaseToken, IValueToken<Operator>
    {
        public Operator Value { get; set; }
        public override TokenKind Kind => TokenKind.Operator;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class BoolLiteralToken : BaseToken, IValueToken<bool>
    {
        public bool Value { get; set; }
        public override TokenKind Kind => TokenKind.BoolLiteral;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class StringLiteralToken : BaseToken, IValueToken<string>
    {
        public string Value { get; set; }
        public override TokenKind Kind => TokenKind.StringLiteral;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }

    public class CharLiteralToken : BaseToken, IValueToken<char>
    {
        public char Value { get; set; }
        public override TokenKind Kind => TokenKind.StringLiteral;
        public override string ToString() => $"{base.ToString()} Value: {Value};";
    }
}
