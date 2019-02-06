using System.Collections.Generic;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Node
{
    public class FuncNode<T> : IFuncNode<T>
    {
        public FuncNode(string identifier, TwisterType? returnType, IScope scope, IList<INode> body)
        {
            Identifier = identifier;
            ReturnType = returnType;
            Scope = scope;
            Body = body;
        }

        public string Identifier { get; }

        public TwisterType? ReturnType { get; }

        public IList<INode> Body { get; set; }

        public NodeKind Kind => NodeKind.Expression;

        public T Value { get; private set; } // TODO

        public IScope Scope { get; }
    }
}
