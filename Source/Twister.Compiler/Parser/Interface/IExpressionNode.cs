using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Interface
{
    public interface IExpressionNode<T> : IValueNode<T>
    {
        ExpressionKind ExpressionKind { get; }
    }
}
