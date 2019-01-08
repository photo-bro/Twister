using System;
using System.Collections.Generic;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Lexer.Enum;

namespace Twister.Compiler.Lexer
{
    public class SourceLexer : ILexer
    {
        private readonly Func<string, ISourceScanner> _createScannerFunc;
        private ISourceScanner _scanner;

        private LexerFlag _flags;

        public SourceLexer(Func<string, ISourceScanner> createSourceReader)
        {
            _createScannerFunc = createSourceReader;
        }

        public IEnumerable<IToken> Tokenize(string sourceCode, LexerFlag flags)
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
            info.TokenType = TokenKind.None;

            char currentChar = _scanner.Advance();
            ConsumeWhiteSpace(ref currentChar);

            switch (currentChar)
            {
                case '+':
                case '-':
                case '%':
                case '*':
                case '/':
                case '\\':
                case '^':
                    info.TokenType = TokenKind.Operator;
                    break;
                case '&':
                    if (_scanner.Peek() == '&')
                        _scanner.Advance();
                    info.TokenType = TokenKind.Operator;
                    break;
                case '|':
                    if (_scanner.Peek() == '|')
                        _scanner.Advance();
                    info.TokenType = TokenKind.Operator;
                    break;
                case '!':
                    if (_scanner.Peek() == '=')
                        _scanner.Advance();
                    info.TokenType = TokenKind.Operator;
                    break;
                case '<':
                    if (_scanner.Peek() == '=' ||
                        _scanner.Peek() == '<')
                        _scanner.Advance();
                    info.TokenType = TokenKind.Operator;
                    break;
                case '>':
                    if (_scanner.Peek() == '=' ||
                        _scanner.Peek() == '>')
                        _scanner.Advance();
                    info.TokenType = TokenKind.Operator;
                    break;
                case '=':
                    {
                        if (_scanner.Peek() == '>')
                        {
                            _scanner.Advance();
                            info.TokenType = TokenKind.Define;
                            break;
                        }
                        if (_scanner.Peek() == '=')
                        {
                            _scanner.Advance();
                            info.TokenType = TokenKind.Operator;
                            break;
                        }
                        info.TokenType = TokenKind.Assign;
                        break;
                    }                
                case ':':
                    info.TokenType = TokenKind.Colon;
                    break;
                case ';':
                    info.TokenType = TokenKind.Semicolon;
                    break;
                case ',':
                    info.TokenType = TokenKind.Comma;
                    break;
                case '.':
                    {
                        var peekChar = _scanner.Peek();
                        if (peekChar == '.')
                        {
                            _scanner.Advance();
                            info.TokenType = TokenKind.DotDot;
                            break;
                        }

                        // Real literals don't have to start with a digit (e.g. .123)
                        if (char.IsDigit(peekChar))
                        {
                            ScanNumeric(ref info, ref currentChar);
                            break;
                        }

                        info.TokenType = TokenKind.Dot;
                        break;
                    }
                case '?':
                    info.TokenType = TokenKind.QuestionMark;
                    break;
                case '\'':
                    ScanCharLiteral(ref info, ref currentChar);
                    break;
                case '\"':
                    ScanStringLiteral(ref info, ref currentChar);
                    break;
                case '(':
                    {
                        if (_scanner.Peek() == '*')
                        {
                            currentChar = _scanner.Advance();
                            ConsumeComment(ref currentChar);
                            return ScanToken(ref info);
                        }

                        info.TokenType = TokenKind.LeftParen;
                        break;
                    }
                case ')':
                    // ScanComment will handle comment endings
                    info.TokenType = TokenKind.RightParen;
                    break;
                case '{':
                    info.TokenType = TokenKind.LeftBrack;
                    break;
                case '}':
                    info.TokenType = TokenKind.RightBrack;
                    break;
                case '[':
                    info.TokenType = TokenKind.LeftSquareBrack;
                    break;
                case ']':
                    info.TokenType = TokenKind.RightSquareBrack;
                    break;
                case '_':
                case char c when char.IsLetter(c):
                    ScanIdentifierOrKeyword(ref info, ref currentChar);
                    break;
                case char c when char.IsDigit(c):
                    ScanNumeric(ref info, ref currentChar);
                    break;
                case char c when c == _scanner.InvalidItem:
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
                    if (!_flags.AllowUnicode())
                        throw new IllegalCharacterException("Character not allowed", _scanner.CurrentSourceLine)
                        { Character = currentChar };
                    throw new FeatureNotSupportedException("Unicode identifiers currently not supported",
                         _scanner.CurrentSourceLine)
                    { FeatureName = "Unicode Identifiers" };
            }

            info.Text = _scanner.CurrentWindow;
            info.SourceLineNumber = _scanner.CurrentSourceLine;
            _scanner.Base = _scanner.Position;
            return true;
        }

        private void ScanIdentifierOrKeyword(ref TokenInfo info, ref char current)
        {
            var cannotBeKeyword = current == '_';

            while (_scanner.Peek().IsTwisterIdentifierOrKeywordChar())
            {
                if (current > 127 && !_flags.AllowUnicode())
                    throw new IllegalCharacterException("Only ASCII characters are currently enabled",
                         _scanner.CurrentSourceLine)
                    { Character = current };

                cannotBeKeyword |= current == '_';
                current = _scanner.Advance();
            }

            // A bit ugly, but we need to check if it's a bool literal
            if ((_scanner.Offset == 4 && _scanner.Peek(-4) == 't') ||
                (_scanner.Offset == 5 && _scanner.Peek(-5) == 'f'))
            {
                info.TokenType = TokenKind.BoolLiteral;
                return;
            }

            // We are aware of '_' in this context and can use that as a determinate for Identifier token
            //
            // Unfortunately there has to be some coupling of the scanning/lexing phase since to determine if an 
            // identifier is a keyword we must have knowledge of the language keywords. In TokenFactory when given
            // a TokenType.Keyword we will check the window value if it is a keyword but default back to identifier if 
            // not.
            info.TokenType = cannotBeKeyword
                ? TokenKind.Identifier
                : TokenKind.Keyword;
        }

        private void ScanNumeric(ref TokenInfo info, ref char current)
        {
            var dotCount = 0;

            while (_scanner.Peek().IsTwisterNumericChar())
            {
                switch (current)
                {
                    case char c when char.IsDigit(c):
                        // short circuit switch statement for common case
                        break;
                    case '.':
                        dotCount++;
                        break;
                    default:
                        throw new UnexpectedCharacterException("Unexpected character found in number",
                            _scanner.CurrentSourceLine)
                        { Character = current };
                }
                current = _scanner.Advance();
            }

            if (char.IsLetter(_scanner.Peek()) || _scanner.Peek() == '_')
                throw new UnexpectedCharacterException("Identifiers cannot begin with a number", _scanner.CurrentSourceLine)
                { Character = current };

            if (current == '.')
                dotCount++;

            if (dotCount > 1)
                throw new UnexpectedCharacterException("Real literal cannot have more than one period",
                    _scanner.CurrentSourceLine)
                { Character = current };

            if (current == 'u' || current == 'U')
            {
                if (dotCount > 0)
                    throw new IllegalCharacterException("Unsigned real numbers are not supported", _scanner.CurrentSourceLine)
                    { Character = '.' };

                info.TokenType = TokenKind.UnsignedInt;
            }
            else if (dotCount > 0)
                info.TokenType = TokenKind.Real;
            else
                info.TokenType = TokenKind.SignedInt;

        }

        private void ScanCharLiteral(ref TokenInfo info, ref char current)
        {
            if (_scanner.Peek(2) == '\'')
            {
                _scanner.Advance(2);
                info.TokenType = TokenKind.CharLiteral;
                return;
            }

            if (_scanner.Peek() == '\\')
            {
                var escapedChar = _scanner.Peek(2);
                if (escapedChar == '\\' || escapedChar == '\'' || escapedChar == '\"' ||
                    escapedChar == 'n' || escapedChar == 'r' || escapedChar == '0')
                {
                    current = _scanner.Advance(2);
                    info.TokenType = TokenKind.CharLiteral;
                    return;
                }
            }

            // empty escaped chars not allowed
            throw new IllegalCharacterException("Empty escaped char literal", _scanner.CurrentSourceLine)
            { Character = '\\' };
        }

        private void ScanStringLiteral(ref TokenInfo info, ref char current)
        {
            current = _scanner.Advance();
            while (current != '\"')
            {
                if (current == '\\' && _scanner.Peek() == '\"')
                    current = _scanner.Advance();

                if (!_flags.AllowUnicode() && current > 127)
                    throw new IllegalCharacterException("Only ASCII characters are currently enabled",
                         _scanner.CurrentSourceLine)
                    { Character = current };

                current = _scanner.Advance();
            }

            info.TokenType = TokenKind.StringLiteral;
        }

        private void ConsumeComment(ref char current)
        {
            current = _scanner.Advance();
            while (current != '*' && _scanner.Peek() != ')')
                current = _scanner.Advance();
            current = _scanner.Advance();
            _scanner.Base = _scanner.Position;
        }

        private void ConsumeWhiteSpace(ref char currentChar)
        {
            // Remove whitespace first
            while (char.IsWhiteSpace(currentChar) || char.IsControl(currentChar))
            {
                _scanner.Base = _scanner.Position;
                currentChar = _scanner.Advance();
            }
        }
    }
}
