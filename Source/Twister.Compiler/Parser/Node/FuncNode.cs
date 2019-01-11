using System.Collections.Generic;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Node
{
    public class FuncNode<T> : IFuncNode<T>
    {
        public FuncNode(string identifier, TwisterType? returnType, IList<IValueNode<ISymbol>> parameters,
             IList<INode> body)
        {
            Identifier = identifier;
            ReturnType = returnType;
            Parameters = parameters;
            Body = body;
        }

        public IList<IValueNode<ISymbol>> Parameters { get; private set; }

        public string Identifier { get; private set; }

        public TwisterType? ReturnType { get; private set; }

        public IList<INode> Body { get; set; }

        public NodeKind Kind => NodeKind.Expression;

        public T Value { get; private set; } // TODO
    }
}
