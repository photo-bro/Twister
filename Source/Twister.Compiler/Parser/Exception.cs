using System;
using Twister.Compiler.Lexer.Enum;
namespace Twister.Compiler.Parser
{
    public class InvalidOperatorException : Exception
    {
        public InvalidOperatorException(string message) : base(message) { }

        public Operator InvalidOperator { get; set; }

        public override string Message => $"{base.Message} Invalid operator: '{InvalidOperator}'";
    }

    public class InvalidComparisonException : Exception
    {
        public string Type { get; set; }

        public InvalidComparisonException(string message) : base(message) { }

        public override string Message => $"{base.Message}. Type: '{Type}'";
    }
}
