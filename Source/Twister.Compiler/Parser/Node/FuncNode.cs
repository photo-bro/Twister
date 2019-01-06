using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Node
{
    public class FuncNode<T> : IFuncNode<T>
    {
        public FuncNode(string identifier, PrimitiveType? returnType, ISymbolNode<TwisterPrimitive>[] arguments,
             INode[] body, IExpressionNode<T> returnExpression)
        {
            Identifier = identifier;
            ReturnType = returnType;
            Arguments = arguments;
            Body = body;
            Value = returnExpression;
        }

        public ISymbolNode<TwisterPrimitive>[] Arguments { get; private set; }

        public string Identifier { get; private set; }

        public PrimitiveType? ReturnType { get; private set; }

        public INode[] Body { get; set; }

        public IExpressionNode<T> Value { get; set; }

        public NodeKind Kind => NodeKind.Expression;
    }
}
