using System;
using System.Linq;
using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Lexer.Token
{
    public static class TokenFactory
    {

        public static IToken Create(ref TokenInfo info, ref LexerFlag flags)
        {
            if (info.TokenType.IsValueToken())
                return CreateValueToken(ref info, ref flags);

            switch (info.TokenType)
            {
                case TokenType.Define:
                    return new DefineToken { LineNumber = info.SourceLineNumber };
                case TokenType.Assign:
                    return new AssignToken { LineNumber = info.SourceLineNumber };
                case TokenType.Colon:
                    return new ColonToken { LineNumber = info.SourceLineNumber };
                case TokenType.Semicolon:
                    return new SemiColonToken { LineNumber = info.SourceLineNumber };
                case TokenType.Comma:
                    return new CommaToken { LineNumber = info.SourceLineNumber };
                case TokenType.Dot:
                    return new DotToken { LineNumber = info.SourceLineNumber };
                case TokenType.DotDot:
                    return new DotDotToken { LineNumber = info.SourceLineNumber };
                case TokenType.QuestionMark:
                    return new QuestionMarkToken { LineNumber = info.SourceLineNumber };
                case TokenType.LeftParen:
                    return new LeftParenToken { LineNumber = info.SourceLineNumber };
                case TokenType.RightParen:
                    return new RightParenToken { LineNumber = info.SourceLineNumber };
                case TokenType.LeftBrack:
                    return new LeftBrackToken { LineNumber = info.SourceLineNumber };
                case TokenType.RightBrack:
                    return new RightBrackToken { LineNumber = info.SourceLineNumber };
                case TokenType.LeftSquareBrack:
                    return new LeftSquareBrackToken { LineNumber = info.SourceLineNumber };
                case TokenType.RightSquareBrack:
                    return new RightSquareBrackToken { LineNumber = info.SourceLineNumber };
                case TokenType.LessThan:
                    return new LessThan { LineNumber = info.SourceLineNumber };
                case TokenType.GreaterThan:
                    return new GreaterThan { LineNumber = info.SourceLineNumber };
                default:
                    return new EmptyToken
                    {
                        LineNumber = info.SourceLineNumber
                    };
            }


        }

        private static IToken CreateValueToken(ref TokenInfo info, ref LexerFlag flags)
        {
            switch (info.TokenType)
            {
                case TokenType.SignedInt:
                    {
                        if (!long.TryParse(info.Text, out var longValue))
                            throw new InvalidTokenException("Unable to parse 64bit signed integer value", info.SourceLineNumber)
                            { InvalidText = info.Text };

                        return new SignedIntToken
                        {
                            Value = longValue,
                            LineNumber = info.SourceLineNumber
                        };
                    }
                case TokenType.UnsignedInt:
                    {
                        if (!ulong.TryParse(info.Text.TrimEnd('u').TrimEnd('U'), out var longValue))
                            throw new InvalidTokenException("Unable to parse 64bit unsigned integer value", info.SourceLineNumber)
                            { InvalidText = info.Text };

                        return new UnsignedIntToken
                        {
                            Value = longValue,
                            LineNumber = info.SourceLineNumber
                        };
                    }
                case TokenType.Real:
                    {
                        if (!double.TryParse(info.Text, out var longValue))
                            throw new InvalidTokenException("Unable to parse double precision floating point value", info.SourceLineNumber)
                            { InvalidText = info.Text };

                        return new RealToken
                        {
                            Value = longValue,
                            LineNumber = info.SourceLineNumber
                        };
                    }
                case TokenType.Keyword:
                case TokenType.Identifier:
                    {
                        var text = info.Text;
                        var keyword = Enum.GetNames(typeof(Keyword)).FirstOrDefault(kw => kw.ToLower() == text);
                        if (keyword != null)
                            return new KeywordToken
                            {
                                Value = Enum.Parse<Keyword>(keyword),
                                LineNumber = info.SourceLineNumber
                            };

                        return new IdToken
                        {
                            Value = info.Text,
                            LineNumber = info.SourceLineNumber
                        };
                    }
                case TokenType.StringLiteral:
                    {
                        var stringValue = info.Text.TrimStart('\"').TrimEnd('\"');
                        return new StringLiteralToken
                        {
                            Value = stringValue,
                            LineNumber = info.SourceLineNumber
                        };
                    }
                case TokenType.CharLiteral:
                    {
                        var rawChar = info.Text.TrimStart('\'').TrimEnd('\'');
                        if (rawChar.Length > 1 && (rawChar[0] != '\\' || rawChar.Length > 2))
                            throw new InvalidTokenException("Char literal too long", info.SourceLineNumber)
                            { InvalidText = rawChar };

                        if (!char.TryParse(rawChar, out var charValue))
                            throw new InvalidTokenException("Char value invalid", info.SourceLineNumber)
                            { InvalidText = rawChar };

                        if (charValue > 127 && !flags.AllowUnicode())
                            throw new IllegalCharacterException("Only ASCII characters are currently supported",
                                info.SourceLineNumber)
                            { Character = charValue };

                        return new CharLiteralToken
                        {
                            Value = charValue,
                            LineNumber = info.SourceLineNumber
                        };
                    }
                case TokenType.Operator:
                    {
                        var text = info.Text;
                        var operatorValue = TokenHelper.OperatorValueMap
                                .FirstOrDefault(kv => kv.Value == text.Trim()).Key;

                        if (operatorValue == default(Operator))
                            throw new InvalidTokenException("Unknown operator found", info.SourceLineNumber)
                            { InvalidText = info.Text };

                        return new OperatorToken
                        {
                            Value = operatorValue,
                            LineNumber = info.SourceLineNumber
                        };
                    }
                default:
                    throw new NotSupportedException($"TokenType {info.TokenType} currently not supported");
            }
        }

    }
}