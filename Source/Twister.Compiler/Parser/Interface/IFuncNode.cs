using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Interface
{
    public interface IFuncNode<T> : IValueNode<IExpressionNode<T>>
    {
        ISymbolNode<TwisterPrimitive>[] Arguments { get; }

        PrimitiveType? ReturnType { get; }

        string Identifier { get; }

        INode[] Body { get; set; }
    }
}
