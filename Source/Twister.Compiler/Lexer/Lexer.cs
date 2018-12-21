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
		private bool _allowUnicode;

		public Lexer(Func<string, ISourceScanner> createSourceReader, string sourceCode, LexerFlag flags)
		{
			_createScannerFunc = createSourceReader;
		}

		public IList<IToken> LexicalAnalysis(string sourceCode, LexerFlag flags)
		{
			_scanner = _createScannerFunc(sourceCode);
			_flags = flags;
			SetFlags();

			var tokenInfo = default(TokenInfo);
			var tokens = new List<IToken>();

			while (ScanForToken(ref tokenInfo))
			{

				var token = TokenFactory.Create(ref tokenInfo);
				tokens.Add(token);
			}

			return tokens;
		}

		private bool ScanForToken(ref TokenInfo info)
		{
			info.Text = string.Empty;
			info.TokenType = TokenType.None;

			var currentChar = _scanner.Advance();
			switch (currentChar)
			{
				case '+':
				case '-':
				case '%':
				case '*':
				case '/':
				case '\\':
					info.TokenType = TokenType.Operator;
					break;
				case '=':
				// handle '=>'
				case '&':
				case '|': // pipe
				case '^':
				case '!':
				case '<':
				// handle '<<'
				case '>':
				// handle '>>' 
				case ':':
				case ';':
				case ',':
				case '.':
				// handle ..
				case '?':
				case '\'':
				case '\"':
				case '(':
				case ')':
				case '{':
				case '}':
				case '[':
				case ']':
					break;
				case char c when char.IsLetter(c):
					LexIdentifierOrKeyword(ref info, c);
					break;
				case char c when char.IsDigit(c):
					LexNumeric(ref info, c);
					break;
				case char c when c == _scanner.InvalidItem:
					// True end of file, exit completely
					if (_scanner.IsAtEnd())
						return false;

					// Check if is just a bad char in source.
					// If not at end and is a bad char don't step the scanning
					// process - exit method normally. Parsing will catch the bad token
					if (_scanner.Position <= _scanner.SourceLength &&
						_scanner.Base < _scanner.SourceLength)
						break;

					// Should be end of file, otherwise it is in a bad state, exit
					return false;
				default:
					throw new IllegalCharacterException("Character not allowed", _scanner.CurrentSourceLine)
					{
						Character = currentChar
					};
			}

			info.Text = _scanner.CurrentWindow;
			info.SourceLineNumber = _scanner.CurrentSourceLine;
			_scanner.Base = _scanner.Position;
			return true;
		}

		private void LexIdentifierOrKeyword(ref TokenInfo info, char current)
		{
			if (!_allowUnicode && current > 128)
				throw new IllegalCharacterException("Only ASCII characters are currently supported",
					 _scanner.CurrentSourceLine)
				{
					Character = current
				};
		}

		private void LexNumeric(ref TokenInfo info, char current)
		{

		}


		private void SetFlags()
		{
			_allowUnicode = (_flags & LexerFlag.AllowUnicode) > 0;
		}
	}
}
