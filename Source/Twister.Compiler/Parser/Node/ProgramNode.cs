using System.Collections.Generic;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Node
{
    public class ProgramNode : IStatementNode
    {
        public ProgramNode(IList<INode> body)
        {
            Value = body;
        }

        public StatementKind StatementKind => StatementKind.Program;

        public IList<INode> Value { get; private set; }

        public NodeKind Kind { get; private set; }
    }
}