using System;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Node
{
    // TODO
    public class ProgramNode : IStatementNode
    {
        public ProgramNode(IFuncNode<TwisterPrimitive>[] body)
        {
            Body = Body;
        }

        public StatementKind StatementKind => StatementKind.Program;

        public INode[] Body { get; private set; }

        public NodeKind Kind { get; private set; }

        public INode Left => null;

        public INode Right => null;
    }
}
