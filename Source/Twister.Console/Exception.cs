using System;
namespace Twister.Console
{
    public class CommandLineArgumentException : Exception
    {
        public string Argument { get; set; }

        public string Suggestion { get; set; }

        public override string Message => $"{base.Message}. Argument: {Argument}";

        public CommandLineArgumentException(string message) : base(message) { }
    }
}
