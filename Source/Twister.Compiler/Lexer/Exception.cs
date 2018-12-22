using System;
namespace Twister.Compiler.Lexer
{
    public abstract class BaseLexerException : Exception
    {
        public int LineNumber { get; set; }

        public BaseLexerException(string message, int lineNumber) : base(message)
        {
            LineNumber = lineNumber;
        }

        public override string Message => $"{base.Message}{Environment.NewLine}\tat source line: ({LineNumber})";

    }

    public class IllegalCharacterException : BaseLexerException
    {
        public char Character { get; set; }

        public IllegalCharacterException(string message, int lineNumber) : base(message, lineNumber) { }

        public override string Message => $"{base.Message} : Char: '{Character}';";
    }


    public class UnexpectedCharacterException : BaseLexerException
    {
        public char Character { get; set; }

        public UnexpectedCharacterException(string message, int lineNumber) : base(message, lineNumber) { }

        public override string Message => $"{base.Message} : Char: '{Character}';";
    }

    public class UnknownKeywordException : BaseLexerException
    {
        public string UnknownKeyword { get; set; }

        public UnknownKeywordException(string message, int lineNumber) : base(message, lineNumber) { }

        public override string Message => $"{base.Message} : Unknown Keyword: {UnknownKeyword};";
    }

    public class InvalidTokenException : BaseLexerException
    {
        public string InvalidText { get; set; }

        public InvalidTokenException(string message, int lineNumber) : base(message, lineNumber) { }

        public override string Message => $"{base.Message} : Invalid text: {InvalidText};";
    }

}
