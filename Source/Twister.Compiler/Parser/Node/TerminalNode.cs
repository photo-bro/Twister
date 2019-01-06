using System;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Node
{
    public class TerminalNode : INode
    {
        public NodeKind Kind => NodeKind.Terminal;
    }
}
