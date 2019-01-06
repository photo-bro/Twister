using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Node
{
    public class ProgramNode : IStatementNode
    {
        public ProgramNode(IFuncNode<TwisterPrimitive>[] body)
        {
            Value = body;
        }

        public StatementKind StatementKind => StatementKind.Program;

        public INode[] Value { get; private set; }

        public NodeKind Kind { get; private set; }
    }
}