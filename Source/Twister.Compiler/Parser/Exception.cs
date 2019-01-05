using System;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Parser.Enum;

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

    public class InvalidCastException : Exception
    {
        public string FromType { get; set; }

        public string ToType { get; set; }

        public InvalidCastException(string message) : base(message) { }

        public override string Message => $"{base.Message}. From type: '{FromType}' to type: '{ToType}'";
    }

    public class InvalidExpressionException : Exception
    {
        public string Left { get; set; }

        public string Right { get; set; }

        public string Operation { get; set; }

        public InvalidExpressionException(string message) : base(message) { }

        public override string Message
            => $"{base.Message}. Left side: '{Left}' Operation: '{Operation}' Right side: '{Right}'";
    }
}
