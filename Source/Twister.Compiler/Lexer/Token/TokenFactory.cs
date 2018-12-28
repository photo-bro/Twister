using System;
using System.Linq;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Enum;

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
                case TokenKind.Define:
                    return new DefineToken { LineNumber = info.SourceLineNumber };
                case TokenKind.Assign:
                    return new AssignToken { LineNumber = info.SourceLineNumber };
                case TokenKind.Colon:
                    return new ColonToken { LineNumber = info.SourceLineNumber };
                case TokenKind.Semicolon:
                    return new SemiColonToken { LineNumber = info.SourceLineNumber };
                case TokenKind.Comma:
                    return new CommaToken { LineNumber = info.SourceLineNumber };
                case TokenKind.Dot:
                    return new DotToken { LineNumber = info.SourceLineNumber };
                case TokenKind.DotDot:
                    return new DotDotToken { LineNumber = info.SourceLineNumber };
                case TokenKind.QuestionMark:
                    return new QuestionMarkToken { LineNumber = info.SourceLineNumber };
                case TokenKind.LeftParen:
                    return new LeftParenToken { LineNumber = info.SourceLineNumber };
                case TokenKind.RightParen:
                    return new RightParenToken { LineNumber = info.SourceLineNumber };
                case TokenKind.LeftBrack:
                    return new LeftBrackToken { LineNumber = info.SourceLineNumber };
                case TokenKind.RightBrack:
                    return new RightBrackToken { LineNumber = info.SourceLineNumber };
                case TokenKind.LeftSquareBrack:
                    return new LeftSquareBrackToken { LineNumber = info.SourceLineNumber };
                case TokenKind.RightSquareBrack:
                    return new RightSquareBrackToken { LineNumber = info.SourceLineNumber };
                case TokenKind.LessThan:
                    return new LessThan { LineNumber = info.SourceLineNumber };
                case TokenKind.GreaterThan:
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
                case TokenKind.SignedInt:
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
                case TokenKind.UnsignedInt:
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
                case TokenKind.Real:
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
                case TokenKind.Keyword:
                case TokenKind.Identifier:
                    {
                        var text = info.Text;
                        var keyword = System.Enum.GetNames(typeof(Keyword)).FirstOrDefault(kw => kw.ToLower() == text);
                        if (keyword != null)
                            return new KeywordToken
                            {
                                Value = System.Enum.Parse<Keyword>(keyword),
                                LineNumber = info.SourceLineNumber
                            };

                        return new IdToken
                        {
                            Value = info.Text,
                            LineNumber = info.SourceLineNumber
                        };
                    }
                case TokenKind.BoolLiteral:
                    {
                        switch (info.Text)
                        {
                            case "true":
                            case "false":
                                if (!bool.TryParse(info.Text, out var boolValue))
                                    throw new InvalidTokenException("Unable to parse bool literal", info.SourceLineNumber)
                                    { InvalidText = info.Text };

                                return new BoolLiteralToken
                                {
                                    Value = boolValue,
                                    LineNumber = info.SourceLineNumber
                                };
                            case string s when s.ToLower() == "true" || s.ToLower() == "false":
                                throw new InvalidTokenException("Unable to parse bool literal", info.SourceLineNumber)
                                { InvalidText = info.Text };
                            default:
                                // Lexer only does a basic check for literal, there is an edge case where an identifer
                                // can sneek past that check
                                return new IdToken
                                {
                                    Value = info.Text,
                                    LineNumber = info.SourceLineNumber
                                };

                        }
                    }
                case TokenKind.StringLiteral:
                    {
                        var stringValue = info.Text.TrimStart('\"').TrimEnd('\"');
                        return new StringLiteralToken
                        {
                            Value = stringValue,
                            LineNumber = info.SourceLineNumber
                        };
                    }
                case TokenKind.CharLiteral:
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
                case TokenKind.Operator:
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