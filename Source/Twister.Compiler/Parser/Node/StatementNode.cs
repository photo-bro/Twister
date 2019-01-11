using System.Collections.Generic;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Node
{
    public class StatementNode : IStatementNode
    {
        public NodeKind Kind => NodeKind.Statement;

        public StatementKind StatementKind => StatementKind.Body;

        public IList<INode> Value { get; private set; }
    }
}
