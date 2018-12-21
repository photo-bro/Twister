using System;

namespace Twister.Compiler.Lexer
{
	[Flags]
	public enum LexerFlag
	{
		None = 0,
		AllowUnicode = None << 1
	}
}
