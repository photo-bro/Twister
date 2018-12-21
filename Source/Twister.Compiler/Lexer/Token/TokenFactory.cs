using System;
using System.Linq;
using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Lexer.Token
{
	public static class TokenFactory
	{

		public static IToken Create(ref TokenInfo info)
		{
			if (info.TokenType.IsValueToken())
				return CreateValueToken(ref info);


			return new Token
			{
				Type = info.TokenType,
				LineNumber = info.SourceLineNumber
			};
		}

		private static IToken CreateValueToken(ref TokenInfo info)
		{
			switch (info.TokenType)
			{
				case TokenType.SignedInt:
					{
						if (!long.TryParse(info.Text, out var longValue))
							throw new InvalidTokenException("Unable to parse 64bit signed integer value", info.SourceLineNumber)
							{
								InvalidText = info.Text
							};
						return new SignedIntToken
						{
							Value = longValue,
							LineNumber = info.SourceLineNumber
						};
					}
				case TokenType.UnsignedInt:
					{
						if (!ulong.TryParse(info.Text, out var longValue))
							throw new InvalidTokenException("Unable to parse 64bit signed integer value", info.SourceLineNumber)
							{
								InvalidText = info.Text
							};
						return new UnsignedIntToken
						{
							Value = longValue,
							LineNumber = info.SourceLineNumber
						};
					}
				case TokenType.Real:
					{
						if (!double.TryParse(info.Text, out var longValue))
							throw new InvalidTokenException("Unable to parse 64bit signed integer value", info.SourceLineNumber)
							{
								InvalidText = info.Text
							};
						return new RealToken
						{
							Value = longValue,
							LineNumber = info.SourceLineNumber
						};
					}
				case TokenType.Identifier:
					return new IdToken
					{
						Value = info.Text,
						LineNumber = info.SourceLineNumber
					};
				case TokenType.Keyword:
					{
						if (!Enum.TryParse<Keyword>(info.Text, out var keywordValue))
							throw new UnknownKeywordException("Unknown keyword found", info.SourceLineNumber)
							{
								UnknownKeyword = info.Text
							};
						return new KeywordToken
						{
							Value = keywordValue,
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
						if (rawChar.Length > 1)
							throw new InvalidTokenException("Char literal too long", info.SourceLineNumber)
							{
								InvalidText = rawChar
							};
						if (!char.TryParse(rawChar, out var charValue))
							throw new InvalidTokenException("Char value invalid", info.SourceLineNumber)
							{
								InvalidText = rawChar
							};
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
							{
								InvalidText = info.Text
							};
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