using System;
using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Lexer
{
	public class TextSourceScanner : ISourceScanner
	{
		private const char InvalidChar = char.MaxValue;
		private readonly string NewLine = Environment.NewLine;
		private readonly string _sourceString;
		private readonly ReadOnlyMemory<char> _source;

		public TextSourceScanner(string source)
		{
			_sourceString = source;
			_source = source.AsMemory();
		}

		public char InvalidItem => InvalidChar;

		public int CurrentSourceLine { get; private set; } = -1;

		public int Offset => Position - Base;

		public int Base { get; set; } = 0;

		public int Position { get; private set; } = -1;

		public int SourceLength => _sourceString.Length;

		public string CurrentWindow => string.Join(string.Empty, _source.Slice(Base, Offset).ToArray());

		public char Advance()
		{
			if (IsAtEnd()) return InvalidChar;
			if (Position + 1 > SourceLength) return InvalidChar;

			return _sourceString[++Position];
		}

		public char Advance(int count)
		{
			if (IsAtEnd()) return InvalidChar;
			if (Position + count > SourceLength) return InvalidChar;

			return _sourceString[Position = Position + count];
		}

		public char PeekNext()
		{
			return Position + 1 < SourceLength
				? _sourceString[Position + 1]
				: InvalidItem;
		}

		public char PeekNext(int count)
		{
			return Position + count < SourceLength
				? _sourceString[Position + count]
				: InvalidItem;
		}

		public bool IsAtEnd()
		{
			return Position >= SourceLength;
		}

		public void Reset()
		{
			CurrentSourceLine = 0;
			Base = 0;
			Position = -1;
		}
	}
}