using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Interface
{
    public interface IExpressionNode<T, U> : IValueNode<T>
    {
        ExpressionKind ExpressionKind { get; }

        Operator Operator { get; }

        IValueNode<U> Left { get; }

        IValueNode<U> Right { get; }
    }
}
