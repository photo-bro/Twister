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


}
