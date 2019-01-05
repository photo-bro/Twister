using System;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Node
{
    public class FuncNode : IFuncNode<TwisterPrimitive>
    {
        public FuncNode(string identifier, ISymbolNode<TwisterPrimitive>[] arguments)
        {
            Identifier = identifier;
            Arguments = arguments;
        }

        public ISymbolNode<TwisterPrimitive>[] Arguments { get; private set; }

        public string Identifier { get; private set; }

        public TwisterPrimitive Value => throw new NotImplementedException(); // TODO

        public NodeKind Kind =>  NodeKind.Expression;

        public INode Left => null;

        public INode Right => null;
    }
}
