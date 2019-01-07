using System.Collections.Generic;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Node
{
    public class FuncNode<T> : IFuncNode<T>
    {
        public FuncNode(string identifier, PrimitiveType? returnType, IList<ISymbolNode<TwisterPrimitive>> arguments,
             IList<INode> body)
        {
            Identifier = identifier;
            ReturnType = returnType;
            Arguments = arguments;
            Body = body;
        }

        public IList<ISymbolNode<TwisterPrimitive>> Arguments { get; private set; }

        public string Identifier { get; private set; }

        public PrimitiveType? ReturnType { get; private set; }

        public IList<INode> Body { get; set; }

        public NodeKind Kind => NodeKind.Expression;

        public TwisterPrimitive Value => throw new System.NotImplementedException(); // TODO
    }
}
