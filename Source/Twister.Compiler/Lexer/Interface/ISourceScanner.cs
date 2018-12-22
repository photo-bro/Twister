namespace Twister.Compiler.Lexer.Interface
{
	public interface ISourceScanner
	{
		char InvalidChar { get; }

		int CurrentSourceLine { get; }

		int Offset { get; }

		int Base { get; set; }

		int Position { get; }

		int SourceLength { get; }

		string CurrentWindow { get; }

		char Advance();

		char Advance(int count);

		char PeekNext();

		char PeekNext(int count);

		bool IsAtEnd();

		void Reset();
	}
}
