using System;
using System.Collections.Generic;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;

namespace Twister.Compiler.Lexer
{
    public class Lexer : ILexer
    {
        private readonly Func<string, ISourceScanner> _createScannerFunc;
        private ISourceScanner _scanner;

        private LexerFlag _flags;

        public Lexer(Func<string, ISourceScanner> createSourceReader)
        {
            _createScannerFunc = createSourceReader;
        }

        public IEnumerable<IToken> LexicalAnalysis(string sourceCode, LexerFlag flags)
        {
            _scanner = _createScannerFunc(sourceCode);
            _flags = flags;

            var tokenInfo = default(TokenInfo);
            var tokens = new List<IToken>();

            while (ScanToken(ref tokenInfo))
            {
                var token = TokenFactory.Create(ref tokenInfo, ref _flags);
                yield return token;
            }
        }

        private bool ScanToken(ref TokenInfo info)
        {
            info.Text = string.Empty;
            info.TokenType = TokenType.None;

            char currentChar = _scanner.Advance();

            // Remove whitespace first
            while (char.IsWhiteSpace(currentChar) || char.IsControl(currentChar))
            {
                currentChar = _scanner.Advance();
                _scanner.Base = _scanner.Position;
            }

            switch (currentChar)
            {
                case '+':
                case '-':
                case '%':
                case '*':
                case '/':
                case '\\':
                case '&':
                case '|':
                case '^':
                case '!':
                    info.TokenType = TokenType.Operator;
                    break;
                case '=':
                    {
                        if (_scanner.Peek() == '>')
                        {
                            _scanner.Advance();
                            info.TokenType = TokenType.Define;
                            break;
                        }

                        info.TokenType = TokenType.Assign;
                        break;
                    }
                case '<':
                    {
                        if (_scanner.Peek() == '<')
                        {
                            _scanner.Advance();
                            info.TokenType = TokenType.Operator;
                            break;
                        }

                        info.TokenType = TokenType.LessThan;
                        break;
                    }
                case '>':
                    {
                        if (_scanner.Peek() == '>')
                        {
                            _scanner.Advance();
                            info.TokenType = TokenType.Operator;
                            break;
                        }

                        info.TokenType = TokenType.GreaterThan;
                        break;
                    }
                case ':':
                    info.TokenType = TokenType.Colon;
                    break;
                case ';':
                    info.TokenType = TokenType.Semicolon;
                    break;
                case ',':
                    info.TokenType = TokenType.Comma;
                    break;
                case '.':
                    {
                        var peekChar = _scanner.Peek();
                        if (peekChar == '.')
                        {
                            _scanner.Advance();
                            info.TokenType = TokenType.DotDot;
                            break;
                        }

                        // Real literals don't have to start with a digit (e.g. .123)
                        if (char.IsDigit(peekChar))
                        {
                            ScanNumeric(ref info, currentChar);
                            break;
                        }

                        info.TokenType = TokenType.Dot;
                        break;
                    }
                case '?':
                    info.TokenType = TokenType.QuestionMark;
                    break;
                case '\'':
                    ScanCharLiteral(ref info);
                    break;
                case '\"':
                    ScanStringLiteral(ref info);
                    break;
                case '(':
                    {
                        if (_scanner.Peek() == '*')
                        {
                            currentChar = _scanner.Advance();
                            ConsumeComment();
                            break;
                        }

                        info.TokenType = TokenType.LeftParen;
                        break;
                    }
                case ')':
                    // ScanComment will handle comment endings
                    info.TokenType = TokenType.RightParen;
                    break;
                case '{':
                    info.TokenType = TokenType.LeftBrack;
                    break;
                case '}':
                    info.TokenType = TokenType.RightBrack;
                    break;
                case '[':
                    info.TokenType = TokenType.LeftSquareBrack;
                    break;
                case ']':
                    info.TokenType = TokenType.RightSquareBrack;
                    break;
                case '_':
                case char c when char.IsLetter(c):
                    ScanIdentifierOrKeyword(ref info, currentChar);
                    break;
                case char c when char.IsDigit(c):
                    ScanNumeric(ref info, currentChar);
                    break;
                case char c when c == _scanner.InvalidChar:
                    // True end of file, exit completely
                    if (_scanner.IsAtEnd())
                        return false;

                    // Check if is just a bad char in source.
                    // If not at end and is a bad char don't stop the scanning
                    // process - exit method normally. Parsing will catch the bad token
                    if (_scanner.Position <= _scanner.SourceLength &&
                        _scanner.Base < _scanner.SourceLength)
                        break;

                    // Should be end of file, otherwise it is in a bad state, exit
                    return false;
                default:
                    throw new IllegalCharacterException("Character not allowed", _scanner.CurrentSourceLine)
                    { Character = currentChar };
            }

            info.Text = _scanner.CurrentWindow;
            info.SourceLineNumber = _scanner.CurrentSourceLine;
            _scanner.Base = _scanner.Position;
            return true;
        }

        private void ScanIdentifierOrKeyword(ref TokenInfo info, char current)
        {
            do
            {
                if (!_flags.AllowUnicode() && current > 127)
                    throw new IllegalCharacterException("Only ASCII characters are currently enabled",
                         _scanner.CurrentSourceLine)
                    { Character = current };

                current = _scanner.Advance();
            } while (char.IsLetterOrDigit(current));

            // Unfortunately there has to be some coupling of the scanning/lexing phase since to determine if an 
            // identifier is a keyword we must have knowledge of the language keywords. In TokenFactory when given
            // a TokenType.Keyword we will check the window value if it is a keyword but default back to identifier if 
            // not.
            info.TokenType = TokenType.Keyword;
        }

        private void ScanNumeric(ref TokenInfo info, char current)
        {
            var dotCount = 0;
            var isUnsignedLiteral = false;

            if (current == '.')
                dotCount++;

            while ((current == '.' ||
                    current == 'U' ||
                    current == 'u' ||
                    char.IsDigit(current)) && !isUnsignedLiteral)
            {
                switch (current)
                {
                    case '.':
                        {
                            dotCount++;
                            if (dotCount > 1)
                                throw new UnexpectedCharacterException("Numeric literal cannot have more than one period",
                                    _scanner.CurrentSourceLine)
                                { Character = current };
                            break;
                        }
                    case 'U':
                    case 'u':
                        isUnsignedLiteral = true;
                        break;
                    case char c when char.IsDigit(c):
                        break;
                }

                current = _scanner.Advance();
            }

            if (isUnsignedLiteral)
            {
                if (dotCount > 0)
                    throw new IllegalCharacterException("Unsigned real numbers are not supported", _scanner.CurrentSourceLine)
                    { Character = '.' };

                info.TokenType = TokenType.UnsignedInt;
            }
            else if (dotCount > 0)
                info.TokenType = TokenType.Real;
            else
                info.TokenType = TokenType.SignedInt;

        }

        private void ScanCharLiteral(ref TokenInfo info)
        {
            if (_scanner.Peek(2) == '\'')
            {
                _scanner.Advance(2);
                info.TokenType = TokenType.CharLiteral;
                return;
            }

            if (_scanner.Peek() == '\\')
            {
                var escapedChar = _scanner.Peek(2);
                if (escapedChar == '\\' || escapedChar == '\'' || escapedChar == '\"' ||
                    escapedChar == '\n' || escapedChar == '\r' || escapedChar == '\0')
                {
                    _scanner.Advance(2);
                    info.TokenType = TokenType.CharLiteral;
                    return;
                }
            }

            // empty escaped chars not allowed
            throw new IllegalCharacterException("Empty escaped char literal", _scanner.CurrentSourceLine)
            { Character = '\\' };
        }

        private void ScanStringLiteral(ref TokenInfo info)
        {
            var current = _scanner.Advance();
            while (current != '\"')
            {
                if (!_flags.AllowUnicode() && current > 127)
                    throw new IllegalCharacterException("Only ASCII characters are currently enabled",
                         _scanner.CurrentSourceLine)
                    { Character = current };
            }

            info.TokenType = TokenType.StringLiteral;
        }

        private void ConsumeComment()
        {
            var current = _scanner.Advance();
            while (current != '*' && _scanner.Peek() != ')')
                current = _scanner.Advance();
            _scanner.Advance(); // ending ')'
        }
    }
}
