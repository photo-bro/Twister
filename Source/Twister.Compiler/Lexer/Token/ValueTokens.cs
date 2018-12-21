using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Lexer.Token
{
	public struct SignedIntToken : IValueToken<long>
	{
		public long Value { get; set; }
		public TokenType Type => TokenType.SignedInt;
		public int LineNumber { get; set; }
	}

	public struct UnsignedIntToken : IValueToken<ulong>
	{
		public ulong Value { get; set; }
		public TokenType Type => TokenType.UnsignedInt;
		public int LineNumber { get; set; }
	}

	public struct RealToken : IValueToken<double>
	{
		public double Value { get; set; }
		public TokenType Type => TokenType.Real;
		public int LineNumber { get; set; }
	}

	public struct IdToken : IValueToken<string>
	{
		public string Value { get; set; }
		public TokenType Type => TokenType.Identifier;
		public int LineNumber { get; set; }
	}

	public struct KeywordToken : IValueToken<Keyword>
	{
		public Keyword Value { get; set; }
		public TokenType Type => TokenType.Keyword;
		public int LineNumber { get; set; }
	}

	public struct OperatorToken : IValueToken<Operator>
	{
		public Operator Value { get; set; }
		public TokenType Type => TokenType.Operator;
		public int LineNumber { get; set; }
	}

	public struct StringLiteralToken : IValueToken<string>
	{
		public string Value { get; set; }
		public TokenType Type => TokenType.StringLiteral;
		public int LineNumber { get; set; }
	}

	public struct CharLiteralToken : IValueToken<char>
	{
		public char Value { get; set; }
		public TokenType Type => TokenType.StringLiteral;
		public int LineNumber { get; set; }
	}
}
