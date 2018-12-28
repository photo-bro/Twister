using System;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Node
{
    public class StatementNode : INode
    {
        public NodeKind Kind => throw new NotImplementedException();

        public INode Left { get; set; }

        public INode Right { get; set; }
    }
}
